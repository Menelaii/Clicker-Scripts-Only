using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private bool _saveProgress;
    [SerializeField] private bool _loadProgress;
    [SerializeField] private PlayerStaticData _playerStaticData;
    [SerializeField] private BodyPartsGenerationStaticData _bodyPartsGenerationSettings;
    [SerializeField] private LocationsStaticData _locationsStaticData;
    [SerializeField] private float _enemySpawnDelay;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Traveler _traveler;

    [Header("Can`t Touch This~")]
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _magicBookButton;
    [SerializeField] private Image _background;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private HealthBar _playerHealthBar;
    [SerializeField] private ManaBar _playerManaBar;
    [SerializeField] private BodyPartsLayout _bodyPartsLayout;
    [SerializeField] private LocationsLayout _locationsLayout;
    [SerializeField] private CoroutineMaster _coroutineMaster;
    [SerializeField] private SetBonusLibrary _setBonusLibrary;
    [SerializeField] private LootPanel _lootPanel;
    [SerializeField] private Canvas _hud;
    [SerializeField] private ItemThrower _itemThrower;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private Transform _optionalPanelsParent;
    [SerializeField] private Prefabs _prefabs;

    private Player _player;
    private EnemySpawner _spawner;
    private GameFactory _gameFactory;
    private Coroutine _spawnerTask;
    private AttackButtonListener _attackButtonListener;
    private MagicBook _magicBook;
    private SpellArgsProvider _spellArgsProvider;

    private Location _currentLocation => _locationsStaticData
        .Locations[_player.Traveler.CurrentLocationIndex];

    private Wave _currentWave => _locationsStaticData
        .Locations[_player.Traveler.CurrentLocationIndex]
        .Waves[_player.Traveler.CurrentWaveIndex];

    private void Awake()
    {
        _coroutineMaster.SetInstance();
        _setBonusLibrary.SetInstance();

        InitPlayer();

        _gameFactory = new GameFactory(_prefabs, _player, _itemThrower, _bodyPartsGenerationSettings, _optionalPanelsParent);

        InitPlayerViews();

        _bodyPartsLayout.Init(_player);
        _locationsLayout.Init(_player, _locationsStaticData);

        _spawner = new EnemySpawner(_gameFactory, _currentLocation.EnemyPosition, _currentWave, _enemySpawnDelay);
        _attackButtonListener = new AttackButtonListener(_playerStaticData.MaxClicksPerSecond);

        var spell = _player.Body.Head.Spells.Count > 0 ? _player.Body.Head.Spells[0] : null;
        _magicBook = new MagicBook(_magicBookButton, _player.Combatant.SpellUser, spell);
        _spellArgsProvider = new SpellArgsProvider(_player.Combatant, _player.Combatant.Target, _gameFactory, _attackButtonListener);
    }

    private void OnEnable()
    {
        _attackButton.onClick.AddListener(_attackButtonListener.OnClick);
        _attackButtonListener.Clicked += _player.Combatant.TryStartAttack;

        _magicBookButton.onClick.AddListener(_magicBook.OnButtonClick);

        _player.Traveler.Traveled += OnTraveled;
        _player.Body.BodyChanged += OnPlayerBodyChanged;
        _player.Wallet.GoldChanged += _walletView.OnGoldChanged;
        _player.Health.ValueChanged += _playerHealthBar.OnValueChanged;
        _player.Combatant.SpellUser.Mana.ValueChanged += _playerManaBar.OnValueChanged;
        _player.Health.Died += OnPlayerDied;
        _player.HealthRegeneration.Changed += _playerHealthBar.OnRegenerationChanged;
        _player.Combatant.SpellUser.ManaRegeneration.Changed += _playerManaBar.OnRegenerationChanged;
        _player.Combatant.TargetChanged += _spellArgsProvider.OnTargetChanged;

        _lootPanel.SellButtonClick += OnLootPanelSellButtonClick;
        _lootPanel.TakeButtonClick += OnLootPanelTakeButtonClick;

        _gameFactory.LootBoxCreated += OnLootBoxCreated;
        _gameFactory.EnemyCreated += OnEnemyCreated;
    }

    private void OnDisable()
    {
        _attackButton.onClick.RemoveListener(_attackButtonListener.OnClick);
        _attackButtonListener.Clicked -= _player.Combatant.TryStartAttack;

        _magicBookButton.onClick.RemoveListener(_magicBook.OnButtonClick);

        _player.Traveler.Traveled -= OnTraveled;
        _player.Body.BodyChanged -= OnPlayerBodyChanged;
        _player.Wallet.GoldChanged -= _walletView.OnGoldChanged;
        _player.Health.ValueChanged -= _playerHealthBar.OnValueChanged;
        _player.Combatant.SpellUser.Mana.ValueChanged -= _playerManaBar.OnValueChanged;
        _player.Health.Died -= OnPlayerDied;
        _player.HealthRegeneration.Changed -= _playerHealthBar.OnRegenerationChanged;
        _player.Combatant.SpellUser.ManaRegeneration.Changed -= _playerManaBar.OnRegenerationChanged;
        _player.Combatant.TargetChanged -= _spellArgsProvider.OnTargetChanged;

        _lootPanel.SellButtonClick -= OnLootPanelSellButtonClick;
        _lootPanel.TakeButtonClick -= OnLootPanelTakeButtonClick;

        _gameFactory.LootBoxCreated -= OnLootBoxCreated;
        _gameFactory.EnemyCreated -= OnEnemyCreated;

        SaveProgress();
    }

    private void Start()
    {
        _player.Traveler.Travel(_player.Traveler.CurrentLocationIndex, _player.Traveler.CurrentWaveIndex);
    }

    public void Open(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void Close(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }

    private void InitPlayer()
    {
        _player = new Player(LoadProgress(), _playerView.GetComponent<Combatant>(), _playerStaticData.AADamageCutPercentage);
    }

    private void InitPlayerViews()
    {
        _walletView.OnGoldChanged(_player.Wallet.Gold);
        _playerHealthBar.OnValueChanged(_player.Health.Value, _player.Health.MaxValue);
        _playerManaBar.OnValueChanged(_player.Combatant.SpellUser.Mana.Value, _player.Combatant.SpellUser.Mana.MaxValue);

        _playerHealthBar.OnRegenerationChanged(_player.HealthRegeneration.PointsPerSecond);
        _playerManaBar.OnRegenerationChanged(_player.Combatant.SpellUser.ManaRegeneration.PointsPerSecond);

        _playerView.GetComponent<PlayerAnimator>().Init(_gameFactory.AttackResultViewer);
    }

    private void OnEnemyCreated(Combatant enemy)
    {
        enemy.DieAnimationEnded += OnEnemyDiedAnimationEnded;
        _player.Combatant.AutoAttacker.Start(() => _player.Combatant.TryStartAttack(_player.Combatant.AutoAttackDamageCut));
    }

    private void OnEnemyDiedAnimationEnded(Combatant enemy)
    {
        enemy.DieAnimationEnded -= OnEnemyDiedAnimationEnded;

        _player.Combatant.AutoAttacker.Stop();

        Destroy(enemy.gameObject);

        if (_spawnerTask != null)
            StopCoroutine(_spawnerTask);

        _spawnerTask = StartCoroutine(_spawner.SpawnEnemyWithWaveDelay());
    }

    private void OnTraveled(int locationIndex, int waveIndex)
    {
        Location location = _locationsStaticData.Locations[locationIndex];
        
        _background.sprite = location.Background;
        _locationsLayout.gameObject.SetActive(false);
        _hud.gameObject.SetActive(true);
        _playerView.SetPosition(location.PlayerPosition);

        if (_spawnerTask != null)
            StopCoroutine(_spawnerTask);

        if (_player.BossesDefeated <= _player.Traveler.CurrentLocationIndex)
        {
            _spawner.SpawnBoss(location.Boss);
        }
        else
        {
            _spawner.SetWave(location.Waves[waveIndex], location.EnemyPosition);
            _spawner.SpawnEnemy();
        }

        UnPause();
    }

    private PlayerProgress LoadProgress()
    {
        var saveSystem = new JsonSaveSystem();
        if (_loadProgress == false || saveSystem.IsSaveFileExist() == false)
        {
            Body body = _playerStaticData.DefaultBody.Body;
            return new PlayerProgress
            {
                Health = body.Corpus.MaxHealth,
                Mana = body.Head.MaxMana,
                BossesDefeated = 1,
                Wallet = _wallet,
                Inventory = new Inventory(_playerStaticData.InventorySize),
                Body = body,
                Traveler = _traveler
            };
        }

        return saveSystem.Load();
    }

    private void SaveProgress()
    {
        if (_saveProgress == false)
            return;

        new JsonSaveSystem().Save(_player.GetProgress());
    }

    private void OnLootBoxCreated(LootBox lootBox)
    {
        lootBox.Opened += OnLootBoxOpened;

        if (_spawnerTask != null)
            StopCoroutine(_spawnerTask);

        _spawner.TryDestroyLastSpawned();

        _attackButton.gameObject.SetActive(false);
    }

    private void OnLootBoxOpened(LootBox lootBox, BodyPart bodyPart)
    {
        lootBox.Opened -= OnLootBoxOpened;

        Pause();

        _player.Inventory.ItemsCountChanged += _lootPanel.OnInventoryItemsCountChanged;
        _lootPanel.Init(bodyPart, _player.Inventory.IsFull);

        _lootPanel.Open();
    }

    private void OnLootPanelSellButtonClick(BodyPart bodyPart)
    {
        _player.Sell(bodyPart);
        OnLootPanelClosed();
    }

    private void OnLootPanelTakeButtonClick(BodyPart bodyPart)
    {
        if (_player.Inventory.IsFull)
            SellFirstItemInInventory();

        _player.Inventory.Add(bodyPart);
        OnLootPanelClosed();
    }

    private void SellFirstItemInInventory()
    {
        _player.Sell(_player.Inventory.BodyParts[0]);
    }

    private void OnLootPanelClosed()
    {
        _player.Inventory.ItemsCountChanged -= _lootPanel.OnInventoryItemsCountChanged;

        _lootPanel.Close();

        _spawnerTask = StartCoroutine(_spawner.SpawnEnemyWithWaveDelay());

        _attackButton.gameObject.SetActive(true);

        UnPause();
    }

    private void OnPlayerDied(Health health)
    {
        _attackButton.interactable = false;
        StartCoroutine(RespawnPlayerWithDelay());

        _player.HealthRegeneration.StopRegeneration();
    }

    private IEnumerator RespawnPlayerWithDelay()
    {
        yield return new WaitForSeconds(_playerStaticData.RespawnDelay);

        _attackButton.interactable = true;
        _player.Health.Heal(_player.Health.MaxValue);
        _player.HealthRegeneration.StartRegeneration();
        _player.Combatant.OnRespawn();
    }

    private void OnPlayerBodyChanged(Body body)
    {
        _player.OnBodyChanged(body);
        _playerView.OnBodyChanged(body);
        _magicBook.OnBodyChanged(body);
    }
}