#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QDebug>
#include "serverStuff.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
     void startServer();
     void stopServer();

    void on_pushButton_testConn_clicked();
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    
private slots:


    void smbConnectedToServer();
    void smbDisconnectedFromServer();
    void gotNewMesssage(QString msg);

private:
    Ui::MainWindow *ui;
    ServerStuff *server;
};

#endif // MAINWINDOW_H
