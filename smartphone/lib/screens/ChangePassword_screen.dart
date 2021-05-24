import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:smartphone/screens/Profile_screen.dart';
import 'package:smartphone/view_models/ChangePassword_viewModel.dart';
import 'package:smartphone/view_models/login_viewModel.dart';
import 'package:stacked/stacked.dart';

class ChangePasswordScreen extends StatelessWidget {
  final TextEditingController currentPass = TextEditingController();
  final TextEditingController newPass = TextEditingController();
  final TextEditingController confirmPass = TextEditingController();
  GlobalKey _form = new GlobalKey<FormState>();
  @override
  Widget build(BuildContext context) {
    return ViewModelBuilder<ChangePasswordView>.nonReactive(
      viewModelBuilder: () => ChangePasswordView(),
      builder: (context, model, child) => WillPopScope(
        onWillPop: () async => false,
        child: Scaffold(
          appBar: AppBar(
            automaticallyImplyLeading: false,
            leading: IconButton(
              icon: Icon(Icons.arrow_back),
              onPressed: () {
                Navigator.of(context).pop();
              },
            ),
            title: Text("Schimbare Parola"),
          ),
          backgroundColor: Colors.white,
          body: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Spacer(),
              Expanded(
                child: Image(
                  width: 300,
                  height: 300,
                  alignment: Alignment.bottomRight,
                  image: NetworkImage(
                      'https://cdn.discordapp.com/attachments/814426440308752449/814460660569735199/1.png'),
                ),
              ),
              Container(
                width: 400,
                decoration: BoxDecoration(color: Colors.grey[100]),
                child: TextFormField(
                  obscureText: true,
                  controller: currentPass,
                  style: GoogleFonts.montserrat(
                    fontSize: 15.5,
                    color: Colors.black,
                  ),
                  decoration: InputDecoration(
                    hintText: "Introduceti parola curenta",
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
                  obscureText: true,
                  controller: newPass,
                  style: GoogleFonts.montserrat(
                    fontSize: 15.5,
                    color: Colors.black,
                  ),
                  decoration: InputDecoration(
                    hintText: "Introduceti parola noua",
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
                  obscureText: true,
                  controller: confirmPass,
                  style: GoogleFonts.montserrat(
                    fontSize: 15.5,
                    color: Colors.black,
                  ),
                  decoration: InputDecoration(
                    hintText: "Confirmati noua parola",
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
                        "Schimba Parola",
                        style: TextStyle(color: Colors.black),
                      ),
                      onPressed: () {
                        model.changePassword(currentPass.text, newPass.text,
                            confirmPass.text, context);
                      })),
              Spacer(),
            ],
          ),
        ),
      ),
    );
  }
}
