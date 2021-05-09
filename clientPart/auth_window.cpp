#include "auth_window.h"
#include "ui_auth_window.h"

auth_window::auth_window(QWidget *parent) :                                 //реализация конструктора
    QWidget(parent),                                                        //список инициализации
    ui(new Ui::auth_window)
{
    ui->setupUi(this);                                                      //вызов функции размещения GUI
    set = new settings();
    connect(set, &auth_window::show, this, &auth_window::login_button_clicked);
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
        QString login = ui->login->text();
        QString pass = ui->pass->text();
        if(login == "123" && pass == "321")
        {
            set->show();
            this->close();
        }

        else {
            QMessageBox::information(this,"Упс","Неправильный логин или пароль");
        }
}

void auth_window::on_registerPushButton_2_clicked()
{

    emit register_button_clicked();
}


