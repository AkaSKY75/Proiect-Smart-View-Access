import cv2
import imutils
import numpy as np
import pytesseract
import socket
import RPi.GPIO as GPIO
import asyncio
import threading
from time import sleep
from PIL import Image

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)
GPIO.setup(22, GPIO.IN, pull_up_down=GPIO.PUD_DOWN)
GPIO.setup(23, GPIO.OUT, initial=GPIO.LOW)
GPIO.setup(24, GPIO.OUT, initial=GPIO.LOW)
GPIO.setup(25, GPIO.OUT, initial=GPIO.LOW)

all_digits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']

all_letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L','M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z']

GPIO.output(23,GPIO.HIGH)

nr_inmatriculare = []

locuri_ocupate = 0
locuri_maxime = 0

def buttonPressed():
    while True:
        if  GPIO.input(22) == GPIO.HIGH:
            sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            sock.settimeout(10)
            message = ""
            try:
                sock.connect(('34.107.39.219', 8000))
                sock.sendall(b'\x01\x01')
                i = 0
                while i != 2:
                    bytes = sock.recv(1)
                    if bytes == b'\x00':
                        i+=1
                        if i == 1:
                            locuri_ocupate = int(message)
                        elif i == 2:
                            locuri_maxime = int(message)
                        message = ""
                    else:
                        message  += bytes.decode('utf-8')
                print('Locuri ocupate: ' + str(locuri_ocupate))
                print('Locuri maxime: ' + str(locuri_maxime))
                sock.close()
            except:
                sock.close()
            GPIO.output(24, GPIO.HIGH)
            sleep(7)
            GPIO.output(24, GPIO.LOW)
        sleep(0.5)
#GPIO.add_event_detect(22, GPIO.FALLING, callback = buttonPressed)

def checkNumberPlate7(parameter):
    a,b,c = parameter[:2], parameter[2:4], parameter[4:]
    if all((a.isalpha(), b.isdigit(), c.isalpha(), len(c) ==3)):
        return 1
    else:
        return 0

def checkNumberPlate7Red(parameter):
    a,b = parameter[:2], parameter[2:]
    if all((a.isalpha(), b.isdigit(), len(b) == 5)):
        return 1
    else:
        return 0

def checkNumberPlate6(parameter):
    a,b = parameter[:2], parameter[2:]
    if all((a.isalpha(), b.isdigit(), len(b) == 4)):
        return 1
    else:
        return 0

def checkBNumberPlate6(parameter):
    a,b,c = parameter[:1], parameter[1:3], parameter[3:]
    if all((a.isalpha(), b.isdigit(), c.isalpha(), len(b) == 2, len(c) == 3)):
        return 1
    else:
        return 0

def checkBNumberPlate6Red(parameter):
    a,b = parameter[:1], parameter[1:]
    if all((a.isalpha(), b.isdigit(), len(b) == 5)):
        return 1
    else:
        return 0

def checkB2NumberPlate7(parameter):
    a,b,c = parameter[:1], parameter[1:4], parameter[4:]
    if all((a.isalpha(), b.isdigit(), c.isalpha(), len(b) == 3, len(c) == 3)):
        return 1
    else:
        return 0

def checkB2NumberPlate7Red(parameter):
    a,b = parameter[:1], parameter[1:]
    if all((a.isalpha(), b.isdigit(), len(b) == 6)):
        return 1
    else:
        return 0
def checkNumberPlate8Red(parameter):
    a,b = parameter[:2], parameter[2:]
    if all((a.isalpha(), b.isdigit(), len(b) == 6)):
        return 1
    else:
        return 0

def my_function(parameter):
    system_enable = True

    if True:
        img = parameter 
        img = cv2.resize(img, (620,480) )
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY) 
        gray = cv2.bilateralFilter(gray, 11, 17, 17) 
        edged = cv2.Canny(gray, 30, 200) 
        cnts = cv2.findContours(edged.copy(), cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
        cnts = imutils.grab_contours(cnts)
        cnts = sorted(cnts, key = cv2.contourArea, reverse = True)[:10]
        screenCnt = None

        for c in cnts:
         peri = cv2.arcLength(c, True)
         approx = cv2.approxPolyDP(c, 0.018 * peri, True)
         if len(approx) == 4:
          screenCnt = approx
          break

        if screenCnt is None:
         detected = 0
         print ('.', end ='', flush=True)
         return 0
        else:
         detected = 1

        if detected == 1:
         cv2.drawContours(img, [screenCnt], -1, (0, 255, 0), 3)

        mask = np.zeros(gray.shape,np.uint8)
        new_image = cv2.drawContours(mask,[screenCnt],0,255,-1,)
        new_image = cv2.bitwise_and(img,img,mask=mask)

        (x, y) = np.where(mask == 255)
        (topx, topy) = (np.min(x), np.min(y))
        (bottomx, bottomy) = (np.max(x), np.max(y))
        Cropped = gray[topx:bottomx+1, topy:bottomy+1]

        text = pytesseract.image_to_string(Cropped, config='--psm 7')
        text = text.replace(" ", "")
        text_new = ""
        for s in text:
            if s in all_digits or s in all_letters:
                text_new += s
            elif s in 'l':
                text_new += '1'
        if len(text_new) == 7:
         verify = checkNumberPlate7(text_new)
         verifyRed = checkNumberPlate7Red(text_new)
         verifyB2 = checkB2NumberPlate7(text_new)
         verifyB2Red = checkB2NumberPlate7Red(text_new)
         if verify == 1 or verifyRed == 1 or verifyB2 == 1 or verifyB2Red == 1:
            print("\nDetected Number is:",text_new)
            sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            sock.settimeout(10)
            try:
                sock.connect(('34.107.39.219', 8000))
                GPIO.output(23,GPIO.HIGH)
                system_enable = True
            except:
                system_enable = False
                nr_inmatriculare.append(text_new)
                GPIO.output(23,GPIO.LOW)
            if system_enable == True:
                bytes = b'\x01\x00'
                bytes += text_new.encode()
                bytes += b'\x00'

                length = 0
                for i in bytes:
                    length += i

                bytes+=length.to_bytes(length=2,byteorder="big")
                
                try:
                    sock.sendall(bytes)
                    message = sock.recv(1)
                except socket.timeout as e:
                    system_enable = False
                    nr_inmatriculare.append(text_new)
                    GPIO.output(23, GPIO.LOW)

                while message == b'\x02':
                    try:
                        sock.sendall(bytes)
                        message = sock.recv(1)
                    except socket.timeout as e:
                        system_enable = False
                        nr_inmatriculare.append(text_new)
                        GPIO.output(23,GPIO.LOW)
                if message == b'\x00':
                  GPIO.output(24,GPIO.HIGH)
                  sleep(7)
                  GPIO.output(24,GPIO.LOW)
                elif message == b'\x01':
                  GPIO.output(25,GPIO.HIGH)
                  sleep(7)
                  GPIO.output(25,GPIO.LOW)
                sock.close()
                return 1
        elif len(text_new) == 6:
         verify = checkNumberPlate6(text_new)
         verifyB = checkBNumberPlate6(text_new)
         verifyBRed = checkBNumberPlate6Red(text_new)
         if verify == 1 or verifyB == 1 or verifyBRed == 1:
            print("\nDetected Number is:",text_new)
            sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            sock.settimeout(10)
            try:
                sock.connect(('34.107.39.219', 8000))
                GPIO.output(23,GPIO.HIGH)
                system_enable = True
            except:
                system_enable = False
                nr_inmatriculare.append(text_new)
                GPIO.output(23,GPIO.LOW)
            if system_enable == True:
                bytes = b'\x01\x00'
                bytes += text_new.encode()

                bytes += b'\x00'

                length = 0
                for i in bytes:
                    length += i

                bytes+=length.to_bytes(length=2,byteorder="big")

                try:
                    sock.sendall(bytes)
                    message = sock.recv(1)
                except socket.timeout as e:
                    system_enable = False
                    nr_inmatriculare.append(text_new)
                    GPIO.output(23, GPIO.LOW)

                while message == b'\x02':
                    try:
                        sock.sendall(bytes)
                        message = sock.recv(1)
                    except socket.timeout as e:
                        system_enable = False
                        nr_inmatriculare.append(text_new)
                        GPIO.output(23,GPIO.LOW)
                if message == b'\x00':
                  GPIO.output(24,GPIO.HIGH)
                  sleep(7)
                  GPIO.output(24,GPIO.LOW)
                elif message == b'\x01':
                  GPIO.output(25,GPIO.HIGH)
                  sleep(7)
                  GPIO.output(25,GPIO.LOW)
                sock.close()
                return 1
        elif len(text_new) == 8:
         verify8Red = checkNumberPlate8Red(text_new)
         if verify8Red == 1:
            print("\nDetected Number is:",text_new)
            sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            sock.settimeout(10)
            try:
                sock.connect(('34.107.39.219', 8000))
                GPIO.output(23,GPIO.HIGH)
                system_enable = True
            except:
                system_enable = False
                nr_inmatriculare.append(text_new)
                GPIO.output(23,GPIO.LOW)
            if system_enable == True:
                bytes = b'\x01\x00'
                bytes += text_new.encode()
                bytes += b'\x00'

                length = 0
                for i in bytes:
                    length += i

                bytes+=length.to_bytes(length=2,byteorder="big")

                try:
                    sock.sendall(bytes)
                    message = sock.recv(1)
                except socket.timeout as e:
                    system_enable = False 
                    nr_inmatriculare.append(text_new)
                    GPIO.output(23, GPIO.LOW)

                while message == b'\x02':
                    try:
                        sock.sendall(bytes)
                        message = sock.recv(1)
                    except socket.timeout as e:
                        system_enable = False
                        nr_inmatriculare.append(text_new)
                        GPIO.output(23,GPIO.LOW)
                if message == b'\x00':
                  GPIO.output(24,GPIO.HIGH)
                  sleep(7)
                  GPIO.output(24,GPIO.LOW)
                elif message == b'\x01':
                  GPIO.output(25,GPIO.HIGH)
                  sleep(7)
                  GPIO.output(25,GPIO.LOW)
                sock.close()
                return 1
def main():
    cap = cv2.VideoCapture('mijloc.mp4')

    x = threading.Thread(target = buttonPressed, args = ())
    x.start()
    while cap.isOpened:
            ret, frame = cap.read()
            ok = my_function(frame)
            if not ret:
                break
            if ok == 1:
                break
    cv2.destroyAllWindows()
    GPIO.output(23,GPIO.LOW)

main()
