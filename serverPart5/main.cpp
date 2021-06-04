#include <QCoreApplication>
#include <QTextCodec>
#include <QtSql/QSqlDatabase>
#include <QtSql/QSqlQuery>
#include <QDebug>
#include "mainwindow.h"

int main(int argc, char *argv[])
{
     QCoreApplication a(argc, argv);

     QSqlDatabase db = QSqlDatabase::addDatabase("QMYSQL");
        db.setHostName("161.97.92.112");
        db.setPort(3306);
        db.setDatabaseName("NeAgario");
        db.setUserName("alex");
        db.setPassword("alexPassword");
        db.open();

    MainWindow w;

    return a.exec();
}
