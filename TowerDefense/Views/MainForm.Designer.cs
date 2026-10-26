namespace TowerDefense.Views;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;
    private Label titleLabel = null!;
    private Label subtitleLabel = null!;
    private GameField gameField = null!;
    private Label legendLabel = null!;
    private Label hintLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        titleLabel = new Label();
        subtitleLabel = new Label();
        gameField = new GameField();
        legendLabel = new Label();
        hintLabel = new Label();
        SuspendLayout();
        // titleLabel
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        titleLabel.Location = new Point(20, 16);
        titleLabel.Text = "Tower Defense";
        // subtitleLabel
        subtitleLabel.AutoSize = true;
        subtitleLabel.Location = new Point(24, 60);
        subtitleLabel.Text = "Неделя 1 · Игровое поле · 12 × 8 клеток";
        // gameField
        gameField.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        gameField.Location = new Point(24, 96);
        gameField.Name = "gameField";
        gameField.Size = new Size(576, 384);
        gameField.TabStop = false;
        // legendLabel
        legendLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        legendLabel.AutoSize = true;
        legendLabel.Location = new Point(24, 500);
        legendLabel.Text = "В — вход врагов     Б — база     Зелёные клетки — места для башен";
        // hintLabel
        hintLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        hintLabel.AutoSize = true;
        hintLabel.Location = new Point(24, 536);
        hintLabel.Text = "Светлая дорога задаёт маршрут от входа к базе.\nВраги и башни появятся на следующем этапе.";
        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(244, 246, 240);
        ClientSize = new Size(624, 592);
        Controls.Add(titleLabel);
        Controls.Add(subtitleLabel);
        Controls.Add(gameField);
        Controls.Add(legendLabel);
        Controls.Add(hintLabel);
        MinimumSize = new Size(640, 631);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tower Defense — неделя 1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
