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
        if(command  == "register"){//обращение к базе данных , еали юзер пытается разегистрироваться
                    QString conf = pieces.value( 3); // получаем подтверждение пароля
                    if(conf==pass){//если пароль и его подтверждение совподает
                            if (sendToClient(clientSocket, QString("%1").arg("successfull registraion user")) == -1){
                                qDebug() << "Some error occured";
                            }else{
                                addNewClient(login,pass);//добовляем нового клиента в базу логинов и паролей
                            }
                    }else{
                        qDebug() << "new client wasn't added to database because password wasn't confirmed";
                            if (sendToClient(clientSocket, QString("%1").arg("unsuccessfull registraion user[login: "+login+" and pass: "+pass+" and confpass: "+conf+"]")) == -1){
                                qDebug() << "Some error occured";
                            }
                    }
        }
    }
}
//добавляем юзера в базу
void ServerStuff::addNewClient(QString login,QString pass){
    //TODO
       //добавить нового клиента с заданным логином и паролем
         qDebug() << "new client was added to database";
}
// проверяем данные пароля и логина с данными из базы данных
bool ServerStuff::checkLogIn(QString login,QString pass){
//TODO
//сделать запрос к базе данных и узнать если клиент с таким логином и паролем


//заглушка
if(login == "1" && pass == "2"){
    qDebug() << "correct login and pass";
    return true;
}
else{
     qDebug() << "uncorrect login and pass";
    return false;
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
