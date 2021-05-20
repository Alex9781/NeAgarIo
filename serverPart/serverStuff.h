#ifndef SERVERSTUFF_H
#define SERVERSTUFF_H

#include <QTcpServer>
#include <QTcpSocket>
#include <QDataStream>
#include <QList>
#include <QtSql/QSqlDatabase>
#include <QtSql/QSqlQuery>
#include <QDebug>

class ServerStuff : public QObject
{
    Q_OBJECT

public:
    ServerStuff(QObject *pwgt);
    QTcpServer *tcpServer;
    QList<QTcpSocket *> getClients();

    void addNewClient(QString login, QString pass);
    bool checkLogIn(QString login, QString pass);

public slots:
    virtual void newConnection();
    void readClient();
    void gotDisconnection();
    qint64 sendToClient(QTcpSocket *socket, const QString &str);

signals:
    void gotNewMesssage(QString msg);
    void ClientDisconnected();

private:
    quint16 m_nNextBlockSize;
    QList<QTcpSocket*> clients;
};

#endif // SERVERSTUFF_H
