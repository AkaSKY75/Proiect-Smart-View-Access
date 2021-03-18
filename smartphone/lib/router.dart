import 'package:auto_route/auto_route_annotations.dart';

import 'package:smartphone/screens/Login_screen.dart';
import 'package:smartphone/screens/qrGenerator_screen.dart';

@MaterialAutoRouter(
  routes: <AutoRoute>[
    // initial route is named "/"
   MaterialRoute(page: Login, initial: true),
   MaterialRoute(page: Qr_Screen),
  ],
)
class $Router {}
