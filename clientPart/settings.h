#ifndef SETTINGS_H
#define SETTINGS_H

#include <QWidget>

namespace Ui {
class settings;
}

class settings : public QWidget
{
    Q_OBJECT

public:
    explicit settings(QWidget *parent = nullptr);
    QString getName();
    QString getSkin();
    ~settings();
signals:
    void startGameClick();

private slots:
    void on_startGame_clicked();

    void on_lineName_textEdited(const QString &arg1);

    void on_lineSkin_textEdited(const QString &arg1);

private:
    Ui::settings *ui;
    QString m_userName;
    QString m_skin;
};

#endif // SETTINGS_H
