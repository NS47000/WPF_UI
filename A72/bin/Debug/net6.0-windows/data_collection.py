import numpy as np
import time
from globalVar import *


colors=colors
fov_h_deg=['-5.6','-2.8','0','2.8','5.6']
fov_v_deg=['-5.6','-2.8','0','2.8','5.6']
direction=sides
spatial_frequency_apd=['16.9cpd','8.45cpd','5.63cpd']
spatial_period_lea_px=['2pixel','4pixel','6pixel']
def split_str(*args):
    output=""
    for item in args:
        output+=str(item)+","
    output+="\n"
    return output
class DUT_data:
    
    DUT_codename="A72"
    serial_number=""
    dataset_name="CTF"
    start_time=time.strftime("%Y-%m-%d %H-%M-%S", time.localtime())
    ctf_result_blue=[]
    ctf_result_green=[]
    ctf_result_red=[]
    ctf_result=np.zeros((3,5,5,2,3),dtype=np.float32)
    
    fixture_name="Ver.1"
    fixture_software_version="Ver.1"
    operator_id=""
    timestamd=""
    
    asset_name=""
    eyebox_x_mm="0"
    eyebox_y_mm="0"
    fov_span_v_deg="0.75"
    fov_span_h_deg="0.75"
    #output_string=""
    
    def __init__(self,SN,operator):
        self.serial_number=SN
        self.operator_id=operator
        #self.color=color
        #self.eyebox_x_mm=eyebox_x
        #self.eyebox_y_mm=eyebox_y
        #self.fov_h_deg=fov_h
        #self.fov_v_deg=fov_v
        #self.fov_span_h_deg=fov_span_h
        #self.fov_span_v_deg=fov_span_v
    def Set_CTF_result(self,ctf_result):
            self.ctf_result=ctf_result
            self.timestamd=str(time.time())

    
    def Save_CSV_file(self):
        file_name="{}_{}_{}_{}.csv".format(self.DUT_codename,self.serial_number,self.dataset_name,self.start_time)
        print(file_name)
        with open(file_name,'w') as f:
            f.write("fixture_name,fixture_software_version,operator_id,timestamd,colors,asset_name,fov_v_deg,fov_h_deg,grill orientation,spatial_frequency_apd,spatial_period_lea_px,ctf_percent,michelson_percentile_min,michelson_percentile_max\n")
            for color_index in range(3):
                for fov_v_index in range(5):
                    for fov_h_index in range(5):
                        for direction_index in range(2):
                            for freq_index in range(3):
                                asset_name="capture_{}_{}_{}.png".format(freq_index+1,direction[direction_index],colors[color_index])
                                f.write(split_str(self.fixture_name,self.fixture_software_version,self.operator_id,self.timestamd,colors[color_index],asset_name,fov_v_deg[fov_v_index],fov_h_deg[fov_h_index],direction[direction_index],spatial_frequency_apd[freq_index],spatial_period_lea_px[freq_index],self.ctf_result[color_index][fov_v_index][fov_h_index][direction_index][freq_index],"21","79"))

        
    
    
                  
    