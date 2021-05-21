import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:smartphone/screens/Profile_screen.dart';
import 'package:smartphone/view_models/login_viewModel.dart';
import 'package:stacked/stacked.dart';

class Login extends StatelessWidget {
  final TextEditingController emailController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  GlobalKey _form = new GlobalKey<FormState>();
  @override
  Widget build(BuildContext context) {
    return ViewModelBuilder<LoginView>.nonReactive(
      viewModelBuilder: () => LoginView(),
      builder: (context, model, child) => WillPopScope(
        onWillPop: () async => false,
        child: Scaffold(
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
                  controller: emailController,
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
                  obscureText: true,
                  controller: passwordController,
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
                      onPressed: () {
                        model.login(emailController.text,
                            passwordController.text, _form, context);
                      })),
              Spacer(),
            ],
          ),
        ),
      ),
    );
  }
}
