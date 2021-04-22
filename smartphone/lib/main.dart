import 'package:flutter/material.dart';
import 'package:smartphone/screens/Login_screen.dart';
import 'package:stacked_services/stacked_services.dart';
// ignore: unused_import
import 'package:firebase_core/firebase_core.dart';
import 'locator.dart';
// ignore: unused_import
import 'package:path_provider/path_provider.dart';
// ignore: unused_import
import 'dart:io';
// ignore: unused_import
import 'dart:async';

void main() {
  //await Firebase.initializeApp();
  setupLocator();
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: Login(),
      navigatorKey: StackedService.navigatorKey,
      debugShowCheckedModeBanner: false,
    );
  }
}
