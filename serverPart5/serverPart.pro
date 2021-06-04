QT +=  network
QT += sql

QT -= gui

CONFIG += c++11 console
CONFIG -= app_bundle

TEMPLATE = app
TARGET = server

MOC_DIR     += generated/mocs

RCC_DIR     += generated/rccs
OBJECTS_DIR += generated/objs

SOURCES += \
    main.cpp \
    mainwindow.cpp \
    serverStuff.cpp

HEADERS += \
    mainwindow.h \
    serverStuff.h

