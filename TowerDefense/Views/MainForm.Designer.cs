namespace TowerDefense.Views;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label titleLabel = null!;
    private Label subtitleLabel = null!;
    private GameField gameField = null!;
    private Label legendLabel = null!;
    private Label hintLabel = null!;
    private Label moneyLabel = null!;
    private Label buildStatusLabel = null!;
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
        subtitleLabel = new Label();
        gameField = new GameField();
        legendLabel = new Label();
        hintLabel = new Label();
        moneyLabel = new Label();
        buildStatusLabel = new Label();
        SuspendLayout();
        // titleLabel
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        titleLabel.Location = new Point(20, 16);
        titleLabel.Text = "Tower Defense";
        // subtitleLabel
        subtitleLabel.AutoSize = true;
        subtitleLabel.Location = new Point(24, 60);
        subtitleLabel.Text = "ЛКМ — построить на траве или выбрать башню";
        // moneyLabel
        moneyLabel.AutoSize = true;
        moneyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        moneyLabel.ForeColor = Color.FromArgb(35, 80, 89);
        moneyLabel.Location = new Point(24, 86);
        moneyLabel.Text = "База: 100/100     Монеты: 150     Цена башни: 50";
        // gameField
        gameField.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        gameField.Location = new Point(24, 120);
        gameField.Name = "gameField";
        gameField.Size = new Size(576, 384);
        gameField.TabStop = false;
        // legendLabel
        legendLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        legendLabel.AutoSize = true;
        legendLabel.Location = new Point(24, 520);
        legendLabel.Text = "В — вход врага     Б — база     Пунктир — радиус атаки башен";
        // hintLabel
        hintLabel.AutoSize = false;
        hintLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        hintLabel.Location = new Point(24, 552);
        hintLabel.Size = new Size(576, 40);
        hintLabel.Text = "Башни автоматически стреляют, когда враг входит в радиус.";
        // buildStatusLabel
        buildStatusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        buildStatusLabel.Location = new Point(24, 600);
        buildStatusLabel.Size = new Size(576, 38);
        buildStatusLabel.Text = "Выберите свободную зелёную клетку для строительства.";
        // updateTimer
        updateTimer.Interval = 20;
        updateTimer.Tick += OnUpdateTimerTick;
        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 246, 242);
        ForeColor = Color.FromArgb(30, 54, 49);
        ClientSize = new Size(624, 652);
        Controls.Add(titleLabel);
        Controls.Add(subtitleLabel);
        Controls.Add(gameField);
        Controls.Add(legendLabel);
        Controls.Add(hintLabel);
        Controls.Add(moneyLabel);
        Controls.Add(buildStatusLabel);
        MinimumSize = new Size(640, 691);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tower Defense";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
