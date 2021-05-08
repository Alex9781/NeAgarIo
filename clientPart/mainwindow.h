#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "auth_window.h"
#include "clientStuff.h"
#include "reg_window.h"
#include "settings.h"

namespace Ui { class MainWindow; }

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    void display();
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    

    void setGameSettings();

    void startgameWithSkinAndName(QString name, QString skin);
public slots:

    void setStatus(bool newStatus);
    void receivedSomething(QString msg);
    void gotError(QAbstractSocket::SocketError err);

private slots:
    void authorizeUser();
    void back_to_auth();
   void startgame();
    void reg();
    void go_to_reg();
    void on_pushButton_send_clicked();
    void on_pushButton_connect_clicked();
    void on_pushButton_disconnect_clicked();

private:
    auth_window ui_Auth;
    reg_window ui_Reg;
    settings Set;
    Ui::MainWindow *ui;
    ClientStuff *client;
};

#endif // MAINWINDOW_H
