#include <QApplication>
#include <QTextCodec>
#include "mainwindow.h"
#include <QProcess>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);

    QProcess::startDetached();

    MainWindow w;
    w.display();
    return a.exec();
}
