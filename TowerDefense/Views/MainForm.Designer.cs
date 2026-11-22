namespace TowerDefense.Views;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label titleLabel = null!;
    private Label phaseLabel = null!;
    private Panel baseCard = null!;
    private Label baseCaptionLabel = null!;
    private Label baseValueLabel = null!;
    private Panel healthTrack = null!;
    private Panel healthFill = null!;
    private Panel moneyCard = null!;
    private Label moneyCaptionLabel = null!;
    private Label moneyValueLabel = null!;
    private GameField gameField = null!;
    private Panel footerPanel = null!;
    private Label summaryLabel = null!;
    private Label buildStatusLabel = null!;
    private Panel sidebarPanel = null!;
    private Label waveCaptionLabel = null!;
    private Label waveLabel = null!;
    private Label enemyCaptionLabel = null!;
    private Label remainingLabel = null!;
    private Label enemyHealthLabel = null!;
    private Label priceCaptionLabel = null!;
    private Label priceValueLabel = null!;
    private Label towerStatsLabel = null!;
    private Button nextWaveButton = null!;
    private Button restartButton = null!;
    private Panel resultPanel = null!;
    private Label resultTitleLabel = null!;
    private Label resultDetailsLabel = null!;
    private System.Windows.Forms.Timer updateTimer = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        updateTimer = new System.Windows.Forms.Timer(components);
        titleLabel = new Label();
        phaseLabel = new Label();
        baseCard = new Panel();
        baseCaptionLabel = new Label();
        baseValueLabel = new Label();
        healthTrack = new Panel();
        healthFill = new Panel();
        moneyCard = new Panel();
        moneyCaptionLabel = new Label();
        moneyValueLabel = new Label();
        gameField = new GameField();
        footerPanel = new Panel();
        summaryLabel = new Label();
        buildStatusLabel = new Label();
        sidebarPanel = new Panel();
        waveCaptionLabel = new Label();
        waveLabel = new Label();
        enemyCaptionLabel = new Label();
        remainingLabel = new Label();
        enemyHealthLabel = new Label();
        priceCaptionLabel = new Label();
        priceValueLabel = new Label();
        towerStatsLabel = new Label();
        nextWaveButton = new Button();
        restartButton = new Button();
        resultPanel = new Panel();
        resultTitleLabel = new Label();
        resultDetailsLabel = new Label();
        SuspendLayout();

        // Заголовок и текущее состояние партии.
        titleLabel.Location = new Point(24, 18);
        titleLabel.Size = new Size(400, 42);
        titleLabel.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        titleLabel.Text = "Tower Defense";
        phaseLabel.Location = new Point(26, 66);
        phaseLabel.Size = new Size(400, 24);
        phaseLabel.ForeColor = Color.FromArgb(105, 121, 131);
        phaseLabel.Text = "Подготовка";

        // Здоровье базы.
        baseCard.Location = new Point(600, 20);
        baseCard.Size = new Size(160, 72);
        baseCard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        baseCard.BackColor = Color.White;
        baseCaptionLabel.Location = new Point(14, 7);
        baseCaptionLabel.Size = new Size(132, 16);
        baseCaptionLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        baseCaptionLabel.ForeColor = Color.FromArgb(105, 121, 131);
        baseCaptionLabel.Text = "БАЗА";
        baseValueLabel.Location = new Point(12, 23);
        baseValueLabel.Size = new Size(136, 34);
        baseValueLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        baseValueLabel.Text = "100 / 100";
        healthTrack.Location = new Point(14, 61);
        healthTrack.Size = new Size(132, 4);
        healthTrack.BackColor = Color.FromArgb(229, 235, 232);
        healthFill.Size = new Size(132, 4);
        healthFill.BackColor = Color.FromArgb(47, 117, 94);
        healthTrack.Controls.Add(healthFill);
        baseCard.Controls.Add(baseCaptionLabel);
        baseCard.Controls.Add(baseValueLabel);
        baseCard.Controls.Add(healthTrack);

        // Деньги.
        moneyCard.Location = new Point(776, 20);
        moneyCard.Size = new Size(160, 72);
        moneyCard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        moneyCard.BackColor = Color.White;
        moneyCaptionLabel.Location = new Point(14, 7);
        moneyCaptionLabel.Size = new Size(132, 16);
        moneyCaptionLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        moneyCaptionLabel.ForeColor = Color.FromArgb(105, 121, 131);
        moneyCaptionLabel.Text = "МОНЕТЫ";
        moneyValueLabel.Location = new Point(12, 23);
        moneyValueLabel.Size = new Size(136, 40);
        moneyValueLabel.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        moneyValueLabel.ForeColor = Color.FromArgb(166, 117, 39);
        moneyValueLabel.Text = "150";
        moneyCard.Controls.Add(moneyCaptionLabel);
        moneyCard.Controls.Add(moneyValueLabel);

        // Поле и краткие результаты действий.
        gameField.Location = new Point(24, 116);
        gameField.Size = new Size(624, 424);
        gameField.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        gameField.Name = "gameField";
        gameField.TabStop = false;
        footerPanel.Location = new Point(24, 556);
        footerPanel.Size = new Size(624, 72);
        footerPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        footerPanel.BackColor = Color.White;
        summaryLabel.Location = new Point(14, 10);
        summaryLabel.Size = new Size(596, 22);
        summaryLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        summaryLabel.Font = new Font("Segoe UI", 10F);
        summaryLabel.ForeColor = Color.FromArgb(105, 121, 131);
        buildStatusLabel.Location = new Point(14, 35);
        buildStatusLabel.AutoSize = true;
        buildStatusLabel.Font = new Font("Segoe UI", 10F);
        footerPanel.Controls.Add(summaryLabel);
        footerPanel.Controls.Add(buildStatusLabel);

        // Показатели волны и кнопки.
        sidebarPanel.Location = new Point(672, 116);
        sidebarPanel.Size = new Size(264, 512);
        sidebarPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        sidebarPanel.BackColor = Color.White;
        waveCaptionLabel.Location = new Point(20, 20);
        waveCaptionLabel.Size = new Size(224, 18);
        waveCaptionLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        waveCaptionLabel.ForeColor = Color.FromArgb(105, 121, 131);
        waveCaptionLabel.Text = "ВОЛНА";
        waveLabel.Location = new Point(16, 42);
        waveLabel.Size = new Size(228, 52);
        waveLabel.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        enemyCaptionLabel.Location = new Point(20, 118);
        enemyCaptionLabel.Size = new Size(224, 18);
        enemyCaptionLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        enemyCaptionLabel.ForeColor = Color.FromArgb(105, 121, 131);
        enemyCaptionLabel.Text = "ОСТАЛОСЬ ВРАГОВ";
        remainingLabel.Location = new Point(17, 139);
        remainingLabel.Size = new Size(224, 42);
        remainingLabel.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
        enemyHealthLabel.Location = new Point(20, 186);
        enemyHealthLabel.Size = new Size(224, 24);
        enemyHealthLabel.ForeColor = Color.FromArgb(105, 121, 131);
        nextWaveButton.Location = new Point(20, 238);
        nextWaveButton.Size = new Size(224, 46);
        nextWaveButton.Text = "Начать волну";
        nextWaveButton.BackColor = Color.FromArgb(47, 117, 94);
        nextWaveButton.ForeColor = Color.White;
        nextWaveButton.FlatStyle = FlatStyle.Flat;
        nextWaveButton.FlatAppearance.BorderSize = 0;
        nextWaveButton.Click += OnNextWaveClick;
        restartButton.Location = new Point(20, 296);
        restartButton.Size = new Size(224, 40);
        restartButton.Text = "Новая игра";
        restartButton.BackColor = Color.FromArgb(239, 243, 243);
        restartButton.FlatStyle = FlatStyle.Flat;
        restartButton.FlatAppearance.BorderSize = 0;
        restartButton.Click += OnRestartClick;
        priceCaptionLabel.Location = new Point(20, 372);
        priceCaptionLabel.Size = new Size(224, 18);
        priceCaptionLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        priceCaptionLabel.ForeColor = Color.FromArgb(105, 121, 131);
        priceCaptionLabel.Text = "БАШНЯ";
        priceValueLabel.Location = new Point(18, 395);
        priceValueLabel.Size = new Size(226, 36);
        priceValueLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        towerStatsLabel.Location = new Point(20, 443);
        towerStatsLabel.Size = new Size(224, 40);
        towerStatsLabel.ForeColor = Color.FromArgb(105, 121, 131);
        sidebarPanel.Controls.Add(waveCaptionLabel);
        sidebarPanel.Controls.Add(waveLabel);
        sidebarPanel.Controls.Add(enemyCaptionLabel);
        sidebarPanel.Controls.Add(remainingLabel);
        sidebarPanel.Controls.Add(enemyHealthLabel);
        sidebarPanel.Controls.Add(nextWaveButton);
        sidebarPanel.Controls.Add(restartButton);
        sidebarPanel.Controls.Add(priceCaptionLabel);
        sidebarPanel.Controls.Add(priceValueLabel);
        sidebarPanel.Controls.Add(towerStatsLabel);

        // Результат партии.
        resultPanel.Size = new Size(440, 210);
        resultPanel.BackColor = Color.White;
        resultPanel.BorderStyle = BorderStyle.FixedSingle;
        resultPanel.Visible = false;
        resultTitleLabel.Location = new Point(24, 26);
        resultTitleLabel.Size = new Size(388, 52);
        resultTitleLabel.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
        resultTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
        resultDetailsLabel.Location = new Point(24, 102);
        resultDetailsLabel.Size = new Size(388, 72);
        resultDetailsLabel.Font = new Font("Segoe UI", 11F);
        resultDetailsLabel.TextAlign = ContentAlignment.TopCenter;
        resultPanel.Controls.Add(resultTitleLabel);
        resultPanel.Controls.Add(resultDetailsLabel);

        updateTimer.Interval = 20;
        updateTimer.Tick += OnUpdateTimerTick;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(232, 238, 240);
        ForeColor = Color.FromArgb(29, 48, 60);
        ClientSize = new Size(960, 652);
        Controls.Add(titleLabel);
        Controls.Add(phaseLabel);
        Controls.Add(baseCard);
        Controls.Add(moneyCard);
        Controls.Add(gameField);
        Controls.Add(footerPanel);
        Controls.Add(sidebarPanel);
        Controls.Add(resultPanel);
        MinimumSize = new Size(976, 691);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tower Defense";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
