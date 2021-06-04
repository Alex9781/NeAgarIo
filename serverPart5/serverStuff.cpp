#include "serverStuff.h"

ServerStuff::ServerStuff(QObject *pwgt) : QObject(pwgt), m_nNextBlockSize(0){
    tcpServer = new QTcpServer(this);
}

QList<QTcpSocket *> ServerStuff::getClients(){return clients;}

//создаем новое подключение
void ServerStuff::newConnection(){
    QTcpSocket *clientSocket = tcpServer->nextPendingConnection();

    connect(clientSocket, &QTcpSocket::disconnected, clientSocket, &QTcpSocket::deleteLater);
    connect(clientSocket, &QTcpSocket::readyRead, this, &ServerStuff::readClient);
    connect(clientSocket, &QTcpSocket::disconnected, this, &ServerStuff::gotDisconnection);

    clients << clientSocket;

    sendToClient(clientSocket, "Reply: connection established");
}

void ServerStuff::readClient(){
    QTcpSocket *clientSocket = (QTcpSocket*)sender();
    QDataStream in(clientSocket);
    for (;;){
        if (!m_nNextBlockSize) {
            if (clientSocket->bytesAvailable() < sizeof(quint16)) { break; }
            in >> m_nNextBlockSize;
        }

        if (clientSocket->bytesAvailable() < m_nNextBlockSize) { break; }
        QString str;
        in >> str;
        emit gotNewMesssage(str); // str - сообщение полученное от клиента
        m_nNextBlockSize = 0;

        QStringList pieces = str.split( " " );
        QString command = pieces.value( 0);// получаем команду сообщенияя
        QString login = pieces.value( 1);// логин
        QString pass = pieces.value( 2);// пароль
        if(command  == "login"){// если юзер присылает сообщение с целью входа в систему
            if(checkLogIn(login,pass)){//обращение к базе данных
                if (sendToClient(clientSocket, QString("%1").arg("pass")) == -1){
                    qDebug() << "Some error occured";
                }
            }
            else{
                if (sendToClient(clientSocket, QString("%1").arg("not pass")) == -1)
                {
                    qDebug() << "Some error occured";
                }
            }
        }
        QString conf = pieces.value( 3); // получаем подтверждение пароля
        if(command  == "register")//обращение к базе данных , еали юзер пытается разегистрироваться
        {
            if(conf==pass) //если пароль и его подтверждение совподает
            {
                if (addNewClient(login,pass))
                {
                    sendToClient(clientSocket, QString("%1").arg("successfull registraion user")) == -1;
                }
                else
                {
                    sendToClient(clientSocket, QString("%1").arg("not successfull registraion user")) == -1;
                }
            }

            else
            {
                qDebug() << "new client wasn't added to database because password wasn't confirmed";
                if (sendToClient(clientSocket, QString("%1").arg("unsuccessfull registraion user[login: "+login+" and pass: "+pass+" and confpass: "+conf+"]")) == -1)
                {
                    qDebug() << "Some error occured";
                }
            }
        }
    }
}
//добавляем юзера в базу
bool ServerStuff::addNewClient(QString login,QString pass){
    QSqlQuery q;
    if (unicLogin(login))
    {
        q.prepare("INSERT INTO accounts (login, password) VALUES (:log, :pass)");
        q.bindValue(":log", login);
        q.bindValue(":pass", pass);
        q.exec();
        qDebug() << "new client was added to database";
        return true;
    }
    else {
        qDebug() << "Its not unical login";
        return false;
    }
}

bool ServerStuff::unicLogin(QString login)
{
    QSqlQuery q;

    q.prepare("SELECT * FROM accounts WHERE login = :log");
    q.bindValue(":log", login);
    if(!q.exec())
    {
        qDebug() <<"error";
    }
    else
    {
        q.first();
        if (q.first())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

// проверяем данные пароля и логина с данными из базы данных
bool ServerStuff::checkLogIn(QString login,QString pass){

    QSqlQuery q;

    q.prepare("SELECT * FROM accounts WHERE login = :log AND password = :pass");
    q.bindValue(":log", login);
    q.bindValue(":pass", pass);
    if(!q.exec())
    {
        qDebug() <<"error";
    }
    else
    {
        q.first();
        QString login1 = q.value(0).toString();
        QString pass1 = q.value(1).toString();
        qDebug() << login1 << pass1;
        if (q.first())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

// удаляем клиента
void ServerStuff::gotDisconnection(){
    clients.removeAt(clients.indexOf((QTcpSocket*)sender()));
    emit ClientDisconnected();
}
//отпрвляем клиенту сообщение по указанному сокету
qint64 ServerStuff::sendToClient(QTcpSocket* socket, const QString& str){
    QByteArray arrBlock;
    QDataStream out(&arrBlock, QIODevice::WriteOnly);
    out << quint16(0) << str;
    out.device()->seek(0);
    out << quint16(arrBlock.size() - sizeof(quint16));

    return socket->write(arrBlock);//происходит собственно сама передача данных
}
