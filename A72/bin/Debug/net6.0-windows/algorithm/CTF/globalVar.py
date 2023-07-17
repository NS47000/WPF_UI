import cv2
import os,stat,sys
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
UI_file_name="UI_status.txt"
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
def show_pattern(img,Dir):
    cv2.imwrite(os.path.join(Dir,"ready.png"),img)
    
    cmdcommand(
               r"call C:\Users\11011105\Anaconda3\condabin\activate.bat",
               r"call conda activate p29_mfg",
               #r"p29-boardctl -d rb3 echo_all",
               r"calibration_client -d rb3 --power on --display on",
               r'calibration_client -d rb3 --show-image "'+os.path.join(Dir,"ready.png")+'"'
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

def show2UI(img,img_message):
    print("show UI image")
    cv2.imwrite(os.path.join(script_dir,"..","..","UI.png"),img)
    result_path=os.path.join(script_dir,"..","..",UI_file_name)
    txt_context=""
    with open(result_path,'r+') as f:
        line=f.readline()
        while line!="" and line is not None:
            if "image imfo." in line:
                txt_context+="image imfo.:{}\n".format(img_message)
            else:
                txt_context+=line
            line=f.readline()
    with open(result_path,'w') as f:
        f.write(txt_context)
    
def init_UI():
    #mode 0:write 1:modify 2:delete 3:clear TXT file
    
    result_path=os.path.join(script_dir,"..","..",UI_file_name)
    with open(result_path,'w') as f:
        f.write("image imfo.:{}\n".format("initial UI"))
        f.write("{},{},{}\n".format("CTF","initial","waiting proccess start"))
        f.write("{},{},{}\n".format("Gamma","initial","waiting proccess start"))
        f.write("{},{},{}\n".format("MTF","initial","waiting proccess start"))
    print("Status update: initial")
    sys.stdout.flush()
def Update_UI(mode,station,status,errorcode):
    #mode 0:write 1:modify 2:delete 3:clear TXT file
    
    result_path=os.path.join(script_dir,"..","..",UI_file_name)
    if mode==0:
        with open(result_path,'a') as f:
            f.write("{},{},{}\n".format(station,status,errorcode))
    elif mode==1:
        txt_context=""
        with open(result_path,'r+') as f:
            line=f.readline()
            while line!="" and line is not None:
                if station in line:
                    txt_context+="{},{},{}\n".format(station,status,errorcode)
                else:
                    txt_context+=line
                line=f.readline()
        with open(result_path,'w') as f:
            f.write(txt_context)
            
    elif mode==2:
        txt_context=""
        with open(result_path,'r+') as f:
            line=f.readline()
            while line!="" and line is not None:
                if station in line:
                    pass
                else:
                    txt_context+=line
                line=f.readline()
        with open(result_path,'w') as f:
            f.write(txt_context)
    elif mode==3:
        init_UI()
    else:
        print("please select correct mode: 0:write 1:modify 2:delete 3:clear TXT file")
    print("Status update:{} {} {}".format(station,status,errorcode))
    sys.stdout.flush()
    

    
    

    