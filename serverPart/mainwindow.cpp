#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{

    server = new ServerStuff(this);
    connect(server, &ServerStuff::gotNewMesssage,
            this, &MainWindow::gotNewMesssage);
    connect(server->tcpServer, &QTcpServer::newConnection,
            this, &MainWindow::smbConnectedToServer);
    connect(server, &ServerStuff::ClientDisconnected,
            this, &MainWindow::smbDisconnectedFromServer);
    startServer();
}

MainWindow::~MainWindow(){
    stopServer(); // стопаем сервер при загкрытие программы
    delete server;
    delete ui;
}

void MainWindow::startServer(){
    if (!server->tcpServer->listen(QHostAddress::Any, 6547)){ // слушаем клаентов с 6547 порт, но возникает ошибка
        qDebug() << "Error!The port is taken by some other service.";
        return;
    }
    connect(server->tcpServer, &QTcpServer::newConnection, server, &ServerStuff::newConnection);// слушаем клаентов с 6547 порт
         qDebug() << "Server started, port is openned.";
}

void MainWindow::stopServer(){
    if(server->tcpServer->isListening()){// если есть слушатели
        disconnect(server->tcpServer, &QTcpServer::newConnection, server, &ServerStuff::newConnection);
        QList<QTcpSocket *> clients = server->getClients();// получаем всех текущих слушателей
        for(int i = 0; i < clients.count(); i++){
            server->sendToClient(clients.at(i), "0");
        }
        server->tcpServer->close();
             qDebug() << "Server stopped, post is closed";
    }
    else{
        qDebug() << "Error! Server was not running";
    }
}

void MainWindow::smbConnectedToServer(){qDebug() << "Somebody has connected";}

void MainWindow::smbDisconnectedFromServer(){qDebug() << "Somebody has disconnected";}

void MainWindow::gotNewMesssage(QString msg){qDebug() << QString("New message: %1").arg(msg);}
