import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:smartphone/screens/Profile_screen.dart';

class Login extends StatefulWidget {
  @override
  _LoginState createState() => _LoginState();
}

class _LoginState extends State<Login> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Spacer(),
          Container(
            width: 400,
            decoration: BoxDecoration(color: Colors.grey[100]),
            child: TextFormField(
              style: GoogleFonts.montserrat(
                fontSize: 15.5,
                color: Colors.black,
              ),
              decoration: InputDecoration(
                hintText: "Introduceti mailul firmei",
                contentPadding: EdgeInsets.only(
                  left: 15,
                ),
                hintStyle: GoogleFonts.montserrat(
                  fontSize: 15.5,
                  color: Colors.grey[400],
                ),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10),
                  borderSide: BorderSide.none,
                ),
              ),
            ),
          ),
          Container(
            height: 50,
          ),
          Container(
            width: 400,
            decoration: BoxDecoration(color: Colors.grey[100]),
            child: TextFormField(
              style: GoogleFonts.montserrat(
                fontSize: 15.5,
                color: Colors.black,
              ),
              decoration: InputDecoration(
                hintText: "Introduceti parola",
                contentPadding: EdgeInsets.only(
                  left: 15,
                ),
                hintStyle: GoogleFonts.montserrat(
                  fontSize: 15.5,
                  color: Colors.grey[400],
                ),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10),
                  borderSide: BorderSide.none,
                ),
              ),
            ),
          ),
          Spacer(),
          Container(
              width: 200,
              decoration: BoxDecoration(
                  color: Colors.grey[100],
                  borderRadius: BorderRadius.circular(10)),
              child: MaterialButton(
                  child: Text(
                    "Login",
                    style: TextStyle(color: Colors.black),
                  ),
                  onPressed: () {})),
          Spacer(),
        ],
      ),
    );
  }
}
