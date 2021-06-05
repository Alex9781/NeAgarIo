#include <QtTest>
#include "main.cpp"

// add necessary includes here

class Tests : public QObject
{
    Q_OBJECT

public:
    Tests();
    ~Tests();

private slots:
    void test_case1();

};

Tests::Tests()
{

}

Tests::~Tests()
{

}

void Tests::test_case1()
{
    QVERIFY(ConnectToDb()==true);
}

QTEST_APPLESS_MAIN(Tests)

#include "tst_tests.moc"
