#include "reg_window.h"
#include "ui_reg_window.h"
#include <QtDebug>

reg_window::reg_window(QWidget *parent) :                               //реализация конструктора окна регистрации
    QWidget(parent),                                                    //список инициализации
    ui(new Ui::reg_window)
{
    ui->setupUi(this);                                                  //установка интерфейса окна регистрации
}

reg_window::~reg_window()                                            //реализация деструктора
{
    delete ui;                                                          //удалить указатель на экземпляр окна регистрации
}




QString reg_window::getName()
{
    return m_userName;
}

QString reg_window::getPass()
{
    return m_userPass;
}

QString reg_window::getConf()
{
    return reg_window::m_confirmation;
}

bool reg_window::checkPass()
{
    return (m_confirmation == m_userPass);
}


void reg_window::on_confirmLineEdit_textEdited(const QString &arg1)
{
    reg_window::m_confirmation = arg1;
}

void reg_window::on_nameLineEdit_textEdited(const QString &arg1)
{
     reg_window::m_userName = arg1;
}

void reg_window::on_passwordLineEdit_textEdited(const QString &arg1)
{
        reg_window::m_userPass = arg1;
}

void reg_window::on_registerPushButton_clicked()
{
     emit register_button_clicked2();
}

void reg_window::on_back_clicked()
{
    emit back_clicked();
}
