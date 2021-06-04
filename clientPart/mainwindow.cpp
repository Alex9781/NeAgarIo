#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) : QMainWindow(parent), ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    ui->pushButton_disconnect->setVisible(false);

    client = new ClientStuff("26.197.4.170", 6547);

    setStatus(client->getStatus());

    connect(client, &ClientStuff::hasReadSome, this, &MainWindow::receivedSomething);
    connect(client, &ClientStuff::statusChanged, this, &MainWindow::setStatus);

    connect(&ui_Auth,SIGNAL(login_button_clicked()),                                 //соединение сигнала кнопки регистрации экземпляра окна авторизации
            this,SLOT(authorizeUser()));
    connect(&ui_Auth,SIGNAL(register_button_clicked()),                                 //соединение сигнала кнопки регистрации экземпляра окна авторизации
            this,SLOT(go_to_reg()));

    connect(&ui_Reg,SIGNAL(back_clicked()),                                 //соединение сигнала кнопки регистрации экземпляра окна авторизации
            this,SLOT(back_to_auth()));

    connect(&Set,SIGNAL(startGameClick()),                                 //соединение сигнала кнопки регистрации экземпляра окна авторизации
            this,SLOT(startgame()));


    connect(&ui_Reg,SIGNAL(register_button_clicked2()),                                 //соединение сигнала кнопки регистрации экземпляра окна авторизации
            this,SLOT(reg()));


    //со слотом вызывающим окно регистрации
  //  connect(&ui_Reg,SIGNAL(register_button_clicked2()),                                 //соединение кнопки регистрации экземпляра окна авторизации



    // FIXME change this connection to the new syntax
    connect(client->tcpSocket, SIGNAL(error(QAbstractSocket::SocketError)),
            this, SLOT(gotError(QAbstractSocket::SocketError)));

     client->connect2host();
}

void MainWindow::startgame()
{
     qDebug() << "start game ";
     startgameWithSkinAndName(Set.getName(),ui_Auth.getLogin());
}
void MainWindow::startgameWithSkinAndName(QString name,QString login)
{
    QStringList p={name,login};
    qDebug() << "start game ";
    QProcess::startDetached("C:/Users/User/Desktop/44444/NeAgarIo.exe", p);
}


void MainWindow::back_to_auth()
{
    ui_Auth.show();
    ui_Reg.hide();
}
void MainWindow::reg()
{

    QByteArray arrBlock;
    QDataStream out(&arrBlock, QIODevice::WriteOnly);

    QString s = "register "+ui_Reg.getName()+" "+ui_Reg.getPass()+" "+ui_Reg.getConf();
    out << quint16(0) <<s;
    out.device()->seek(0);
    out << quint16(arrBlock.size() - sizeof(quint16));
    client->tcpSocket->write(arrBlock);


}
void MainWindow::go_to_reg()
{
    ui_Auth.hide();
    ui_Reg.show();
}


void MainWindow::authorizeUser()
{


    QByteArray arrBlock;
    QDataStream out(&arrBlock, QIODevice::WriteOnly);
    //out.setVersion(QDataStream::Qt_5_10);


    QString s = "login "+ui_Auth.getLogin()+" "+ui_Auth.getPass();
    out << quint16(0) <<s;
    out.device()->seek(0);
    out << quint16(arrBlock.size() - sizeof(quint16));
    client->tcpSocket->write(arrBlock);


}
MainWindow::~MainWindow()
{
    delete client;
    delete ui;
}
void MainWindow::display()                                                              //реализация пользотвальского метода отображения главного окна
{
    ui_Auth.show();                                                                     //отобразить окно авторизации(НЕ главное окно)
}
void MainWindow::setStatus(bool newStatus)
{
    if(newStatus)
    {
        ui->label_status->setText(
                    tr("<font color=\"green\">CONNECTED</font>"));
        ui->pushButton_connect->setVisible(false);
        ui->pushButton_disconnect->setVisible(true);
    }
    else
    {
        ui->label_status->setText(
                tr("<font color=\"red\">DISCONNECTED</font>"));
        ui->pushButton_connect->setVisible(true);
        ui->pushButton_disconnect->setVisible(false);
    }
}

void MainWindow::receivedSomething(QString msg)
{

    ui->textEdit_log->append(msg);
    qDebug() << msg;
    if(msg=="pass"){
        ui_Auth.hide();
        setGameSettings();
    }
    if(msg == "successfull registraion user"){
        //ui_Reg.hide();
        //setGameSettings();
        back_to_auth();
    }
    if (msg == "not successfull registraion user")
    {
        QMessageBox::warning(this, "Внимание","такой логин уже есть");
    }


}

void MainWindow::setGameSettings(){
    Set.show();
    qDebug() << "start game settings ";

}


void MainWindow::gotError(QAbstractSocket::SocketError err)
{
    //qDebug() << "got error";
    QString strError = "unknown";
    switch (err)
    {
        case 0:
            strError = "Connection was refused";
            break;
        case 1:
            strError = "Remote host closed the connection";
            break;
        case 2:
            strError = "Host address was not found";
            break;
        case 5:
            strError = "Connection timed out";
            break;
        default:
            strError = "Unknown error";
    }

    ui->textEdit_log->append(strError);
}

void MainWindow::on_pushButton_connect_clicked()
{
//    client->connect2host();
}

void MainWindow::on_pushButton_send_clicked()
{
    QByteArray arrBlock;
    QDataStream out(&arrBlock, QIODevice::WriteOnly);
    //out.setVersion(QDataStream::Qt_5_10);
    out << quint16(0) << ui->lineEdit_message->text();

    out.device()->seek(0);
    out << quint16(arrBlock.size() - sizeof(quint16));

    client->tcpSocket->write(arrBlock);
}

void MainWindow::on_pushButton_disconnect_clicked()
{
    client->closeConnection();
}
