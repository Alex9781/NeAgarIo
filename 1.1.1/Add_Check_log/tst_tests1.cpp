#include <QtTest>
#include <serverStuff.cpp>
// add necessary includes here

class Tests1 : public QObject
{
    Q_OBJECT

public:
    Tests1();
    ~Tests1();

private slots:
    void test_case1();
    void test_case2();
    void test_case3();


};

Tests1::Tests1()
{
    QSqlDatabase db = QSqlDatabase::addDatabase("QMYSQL");
       db.setHostName("161.97.92.112");
       db.setPort(3306);
       db.setDatabaseName("NeAgario");
       db.setUserName("alex");
       db.setPassword("alexPassword");
       db.open();
}

Tests1::~Tests1()
{
    QSqlDatabase db = QSqlDatabase::addDatabase("QMYSQL");
    db.close();
}

void Tests1::test_case1()
{
    QVERIFY( ServerStuff::unicLogin("123")==false);
}

void Tests1::test_case2()
{
    QVERIFY(ServerStuff::addNewClient("zxc","321")==true);
}
void Tests1::test_case3()
{
    QVERIFY(ServerStuff::checkLogIn("zxc","321")==true);
}


QTEST_APPLESS_MAIN(Tests1)

#include "tst_tests1.moc"
