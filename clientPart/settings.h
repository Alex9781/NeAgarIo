#ifndef SETTINGS_H
#define SETTINGS_H

#include <QWidget>
#include <auth_window.h>

namespace Ui {
class settings;
}

class settings : public QWidget
{
    Q_OBJECT

public:
    explicit settings(QWidget *parent = nullptr);
    QString getName();
    QString getLogin();
    ~settings();
signals:
    void startGameClick();

private slots:
    void on_startGame_clicked();

    void on_lineName_textEdited(const QString &arg1);


private:
    Ui::settings *ui;
    QString m_userName;
    QString m_login;
};

#endif // SETTINGS_H
