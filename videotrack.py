# -*- coding: utf-8 -*-
"""
Created on Fri Jun 16 09:43:29 2017

@author: PhucMinh
"""

#import cv2
#import numpy as np
#drawing = False # true if mouse is pressed
#mode = True # if True, draw rectangle. Press 'm' to toggle to curve
#ix,iy = -1,-1
## mouse callback function
#def draw_circle(event,x,y,flags,param):
#    global ix,iy,drawing,mode
#    if event == cv2.EVENT_LBUTTONDOWN:
#        drawing = True
#        ix,iy = x,y
#        cv2.imshow('image',img)
#    elif event == cv2.EVENT_MOUSEMOVE:
#        if drawing == True:
#            if mode == True:
#                imgcopy=np.copy(img)
#                cv2.rectangle(imgcopy,(ix,iy),(x,y),(0,255,0),3)
#                cv2.imshow('image',imgcopy)
#            else:
#                imgcopy=np.copy(img)
#                cv2.circle(imgcopy,(x,y),5,(0,0,255),-1)
#                cv2.imshow('image',imgcopy)
#    elif event == cv2.EVENT_LBUTTONUP:
#        drawing = False
#        if mode == True:
#            imgcopy=np.copy(img)
#            cv2.rectangle(imgcopy,(ix,iy),(x,y),(0,255,0),3)
#            cv2.imshow('image',imgcopy)
#        else:
#            imgcopy=np.copy(img)
#            cv2.circle(imgcopy,(x,y),5,(0,0,255),-1)
#            cv2.imshow('image',imgcopy)
#            
#img = np.zeros((512,512,3), np.uint8)
#imgorg = np.copy(img)
#cv2.namedWindow('image')
#cv2.setMouseCallback('image',draw_circle)
#cv2.imshow('image',img)
#while(1):
#    k = cv2.waitKey(1) & 0xFF
#    if k == ord('m'):
#        mode = not mode
#    elif k == 27:
#        break
#cv2.destroyAllWindows()
#
#events = [i for i in dir(cv2) if 'EVENT' in i]
#print( events )

import cv2
import numpy as np
import time
import sys


vid = cv2.VideoCapture('dance.mp4')
num_f = int(vid.get(cv2.CAP_PROP_FRAME_COUNT)) #Number of frames
fps = int(vid.get(cv2.CAP_PROP_FPS))
obj = []
RET =[]
t=time.time()
for i in range(num_f):
    ret, frame = vid.read()
    obj.append(frame)
    RET.append(ret)
    if time.time()-t>1:
        a=np.ceil(i*100*1.0/num_f)
        b = ("reading video ... " + "%d"%a+"%")
        sys.stdout.write('\r'+b)
        t=time.time()
sys.stdout.write('\r'+"reading video ... 100%") 
print ""
time.sleep(1)
vid.release()




def nothing(x):
    pass

# Create a black image, a window
img = np.zeros((300,512,3), np.uint8)
cv2.namedWindow('image')

# create trackbars for color change
cv2.createTrackbar('frame','image',0,num_f-1,nothing)

i=0
while(1):
    cv2.imshow('image',img)
    k = cv2.waitKey(1) & 0xFF
    if k == 13:
        break
    if k == 112:
        while RET[i]==True and i<=num_f:
            k = cv2.waitKey(1000/fps) & 0xFF
            if k == 112:
                break
            img=np.copy(obj[i])
            font = cv2.FONT_HERSHEY_COMPLEX_SMALL
            cv2.putText(img,"Press on 'p' to Play/Pause",(5,10), font, 0.6,(255,255,255),1,cv2.LINE_4)
            cv2.imshow('image',img)
            i=i+1
            cv2.setTrackbarPos('frame','image',i)
    # get current positions of four trackbars
    i = cv2.getTrackbarPos('frame','image')
    img=np.copy(obj[i])
    font = cv2.FONT_HERSHEY_COMPLEX_SMALL
    cv2.putText(img,'Choose frame and press Enter',(5,25), font, 0.6,(255,255,255),1,cv2.LINE_4)
    cv2.putText(img,"Press on 'p' to Play/Pause",(5,10), font, 0.6,(255,255,255),1,cv2.LINE_4)
    

cv2.destroyAllWindows()

drawing = False # true if mouse is pressed
ix,iy = -1,-1
# mouse callback function
def draw_circle(event,x,y,flags,param):
    global ix,iy,drawing
    if event == cv2.EVENT_LBUTTONDOWN:
        drawing = True
        ix,iy = x,y
        cv2.imshow('image',img)
    elif event == cv2.EVENT_MOUSEMOVE:
        if drawing == True:
            imgcopy=np.copy(img)
            cv2.rectangle(imgcopy,(ix,iy),(x,y),(0,255,0),3)
            cv2.imshow('image',imgcopy)
    elif event == cv2.EVENT_LBUTTONUP:
        drawing = False
        imgcopy=np.copy(img)
        cv2.rectangle(imgcopy,(ix,iy),(x,y),(0,255,0),3)
        cv2.imshow('image',imgcopy)
            
img = obj[i]
cv2.putText(img,'Draw a bounding box and press Enter',(5,10), font, 0.6,(255,255,255),1,cv2.LINE_4)
cv2.namedWindow('image')
cv2.setMouseCallback('image',draw_circle)
cv2.imshow('image',img)
while(1):
    k = cv2.waitKey(1) & 0xFF
    if k == 13:
        break
cv2.destroyAllWindows()

text_file = open("Output.txt", "w")
for i in range(10):
    text_file.write("%d"%i+'\n')
    
text_file.close()

