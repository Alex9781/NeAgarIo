#ifndef MAINWINDOW_H
#define MAINWINDOW_H


#include <QDebug>
#include "serverStuff.h"


class MainWindow : public QObject
{
    Q_OBJECT
    
public:
     void startServer();
     void stopServer();

    void on_pushButton_testConn_clicked();
    ~MainWindow();

    MainWindow();
private slots:


    void smbConnectedToServer();
    void smbDisconnectedFromServer();
    void gotNewMesssage(QString msg);

private:

    ServerStuff *server;
};

#endif // MAINWINDOW_H
