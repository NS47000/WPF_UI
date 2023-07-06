import cv2
import os,stat
import numpy as np
import math
sides=['v','h']
colors=['B','G','R']
pattern_width=640
pattern_height=480
script_dir =  os.path.abspath(os.path.dirname(__file__))
pattern_folder=os.path.join(script_dir,"pattern")
capture_folder=os.path.join(script_dir,"capture")
process_folder=os.path.join(script_dir,"process")
def check_folder(folder_path):
    if os.path.exists(folder_path)==0:
        os.mkdir(folder_path)
        os.chmod(folder_path,stat.S_IWRITE)
        print("創建{}資料夾".format(os.path.basename(folder_path)))

def cmdcommand(*input):
    with open('command.bat', 'w') as f:
        for i in range(0,len(input)):
            f.write(input[i]+"\n")
    
    os.system("command.bat")
def show_pattern(img):
    cv2.imwrite(os.path.join(script_dir,"ready.png"),img)
    
    cmdcommand(
               r"call C:\Users\11011105\Anaconda3\condabin\activate.bat",
               r"call conda activate p29_mfg",
               #r"p29-boardctl -d rb3 echo_all",
               r"calibration_client -d rb3 --power on --display on",
               r'calibration_client -d rb3 --show-image "'+os.path.join(script_dir,"ready.png")+'"'
               )

def close():
    cmdcommand(
                r"call C:\Users\11011105\Anaconda3\condabin\activate.bat",
                r"call conda activate p29_mfg",
                r"calibration_client -d rb3 --power off --display off"
               )
def show_image(img):
    cv2.namedWindow('Show Mason Image', cv2.WINDOW_NORMAL)
    cv2.imshow('Show Mason Image', img)
    cv2.waitKey(0)
    cv2.destroyAllWindows()
    
def show2UI(img,name):
    print("show UI image:"+name)
    cv2.imwrite(os.path.join(script_dir,"show.png"),img)
    os.chmod(script_dir,stat.S_IWRITE)
    