#include "settings.h"
#include "ui_settings.h"

settings::settings(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::settings)
{
    ui->setupUi(this);
}

settings::~settings()
{
    delete ui;
}


QString settings::getSkin()
{
    return settings::m_skin;
}

QString settings::getName()
{
    return settings::m_userName;
}

void settings::on_startGame_clicked()
{
    emit startGameClick();
}

void settings::on_lineName_textEdited(const QString &arg1)
{
    m_userName = arg1;
}

void settings::on_lineSkin_textEdited(const QString &arg1)
{
     m_skin = arg1;
}
