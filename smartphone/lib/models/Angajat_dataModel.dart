class AngajatDataModel {
  String avatar;
  int birou;
  String cnp;
  String departament;
  String email;
  String email_firma;
  int etaj;
  int loc;
  String numar_inmatriculare;
  String nume;
  String parola;
  String prenume;
  String uid;
  AngajatDataModel(
      {this.avatar,
      this.birou,
      this.cnp,
      this.departament,
      this.email,
      this.email_firma,
      this.etaj,
      this.loc,
      this.numar_inmatriculare,
      this.nume,
      this.parola,
      this.prenume,
      this.uid});
  static AngajatDataModel toMap(avatar, birou, cnp, departament, email,
      email_firma, etaj, loc, numar_inmatriculare, nume, parola, prenume) {
    return AngajatDataModel(
        avatar: avatar,
        birou: birou,
        cnp: cnp,
        departament: departament,
        email: email,
        email_firma: email_firma,
        etaj: etaj,
        loc: loc,
        numar_inmatriculare: numar_inmatriculare,
        nume: nume,
        parola: parola,
        prenume: prenume);
  }

  static AngajatDataModel fromMap(
      Map<String, dynamic> map, String downloadUrl, String uid) {
    if (map == null) {
      print("NOTGOOD");
      return null;
    }
    return AngajatDataModel(
        avatar: downloadUrl,
        birou: map["birou"],
        cnp: map["cnp"],
        departament: map["departament"],
        email: map["email"],
        email_firma: map["email_firma"],
        etaj: map["etaj"],
        loc: map["loc"],
        numar_inmatriculare: map["numar_inmatriculare"],
        nume: map["nume"],
        parola: map["parola"],
        prenume: map["prenume"],
        uid: uid);
  }

  String get _avatar => avatar;
  int get _birou => birou;
  String get _cnp => cnp;
  String get _departament => departament;
  String get _email => email;
  String get _email_firma => email_firma;
  int get _etaj => etaj;
  int get _loc => loc;
  String get _numar_inmatriculare => numar_inmatriculare;
  String get _nume => nume;
  String get _prenume => prenume;
  String get _uid => uid;
}
