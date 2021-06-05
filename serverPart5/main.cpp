#include <QCoreApplication>
#include <QTextCodec>
#include <QtSql/QSqlDatabase>
#include <QtSql/QSqlQuery>
#include <QDebug>
#include "mainwindow.h"

bool ConnectToDb()
{
    QSqlDatabase db = QSqlDatabase::addDatabase("QMYSQL");
       db.setHostName("161.97.92.112");
       db.setPort(3306);
       db.setDatabaseName("NeAgario");
       db.setUserName("alex");
       db.setPassword("alexPassword");
       return db.open();

}

int main(int argc, char *argv[])
{
     QCoreApplication a(argc, argv);

    ConnectToDb();

    MainWindow w;

    return a.exec();
}
