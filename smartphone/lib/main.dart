import 'package:flutter/material.dart';
import 'package:smartphone/screens/ChangePassword_screen.dart';
import 'package:smartphone/screens/Login_screen.dart';
import 'package:smartphone/screens/Profile_screen.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:smartphone/services/ChangePassword_service.dart';
import 'locator.dart';


void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp();
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
      debugShowCheckedModeBanner: false,
      initialRoute: '/',
      routes: {
        '/': (context) => Login(),
        '/profile': (context) => ProfileScreen(),
        '/changePassword': (context) => ChangePasswordScreen(),
      },
    );
  }
}
