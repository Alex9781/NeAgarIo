#include "auth_window.h"
#include "ui_auth_window.h"

auth_window::auth_window(QWidget *parent) :                                 //реализация конструктора
    QWidget(parent),                                                        //список инициализации
    ui(new Ui::auth_window)
{
    ui->setupUi(this);                                                      //вызов функции размещения GUI
}

auth_window::~auth_window()                                                 //реализация деструктора
{
    delete ui;                                                              //удалить указатель на экземпляр класса(уничтожить GUI)
}

QString auth_window::getLogin()
{
    return auth_window::m_username;
}

QString auth_window::getPass()
{
    return auth_window::m_userpass;
}

void auth_window::on_lineEdit_textEdited(const QString &arg1)
{
        auth_window::m_username = arg1;
}

void auth_window::on_lineEdit_2_textEdited(const QString &arg1)
{

 auth_window::m_userpass = arg1;
}
void auth_window::on_loginPushButton_clicked()
{
        emit login_button_clicked();
}

void auth_window::on_registerPushButton_2_clicked()
{

    emit register_button_clicked();
}


