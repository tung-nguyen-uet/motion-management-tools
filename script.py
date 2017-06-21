# -*- coding: utf-8 -*-
"""
Created on Tue Jun 06 10:47:42 2017

@author: PhucMinh
"""

#import numpy as np
#import cv2
#import matplotlib.pyplot as plt
#img = cv2.imread('cameraman.png',0)

#laplacian=cv2.Laplacian(img,cv2.CV_64F)
#
#ha=np.abs(laplacian)
#
#plt.subplot(2,1,1)
#plt.hist(ha.ravel(),544,[0,543])
#plt.subplot(2,1,2)
#plt.hist(ha.ravel(),10,[0,543])
#plt.show()
#
#[m,n]=ha.shape
#he=np.zeros((m,n),dtype=np.uint8)
#
#for i in range(m):
#    for j in range(n):
#        if ha[i,j]>15:
#            he[i,j]=255
#
#
#
#cv2.imshow('by',he)
#cv2.waitKey(0)
#cv2.destroyAllWindows()




#import numpy as np
#import cv2
#cap = cv2.VideoCapture('dance.mp4')
#nf = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
#for i in range(nf):
#    ret, frame = cap.read()
#    if ret==True:
#        cv2.imshow('frame',frame)
#        cv2.waitKey(50)
#cap.release()
#cv2.destroyAllWindows()







#import numpy as np
#import cv2
#cap = cv2.VideoCapture('dance.mp4')
## Define the codec and create VideoWriter object
#
#X=[]
#for i in range(20):
#    ret, frame = cap.read()
#    if ret==True:
#        X.append(frame)
#    else:
#        break
## Release everything if job is finished
#cap.release()
#
#fourcc = cv2.VideoWriter_fourcc(*'XVID')
#out = cv2.VideoWriter('output.avi',fourcc, 5.0, (320,240))
#
#for i in range(len(X)):
#    out.write(X[i])
#
#out.release()
#cv2.destroyAllWindows()




import cv2
import numpy as np
#import os
import time
import sys


def absdif(I,J):
    k = cv2.cvtColor(I, cv2.COLOR_BGR2GRAY)
    l = cv2.cvtColor(J, cv2.COLOR_BGR2GRAY)
    m = cv2.calcHist([k], [0], None, [256], [0, 256])
    n = cv2.calcHist([l], [0], None, [256], [0, 256])
    dif = np.abs(m - n)
    res = np.sum(dif)
    return res


#SETUP VIDEO READER
fpk = 20; #Number of frame per keyframe
vid = cv2.VideoCapture('dance.mp4')
width = int(vid.get(cv2.CAP_PROP_FRAME_WIDTH))
height = int(vid.get(cv2.CAP_PROP_FRAME_HEIGHT))
num_f = int(vid.get(cv2.CAP_PROP_FRAME_COUNT)) #Number of frames
num_key = int(np.ceil(num_f*1.0/fpk))
obj = []
RET = []
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


#SETUP VIDEO WRITER
fourcc = cv2.VideoWriter_fourcc(*'DIVX')
obj_sum = cv2.VideoWriter('HisSummaryFTU.avi', fourcc, 2.5, (width, height))


#STATISTICAL PROCESSING
#Loop over all frame
X = []
k=0
retk=RET[1]
RET.append(False)
t=time.time()
while retk==True and k<=num_f-2:
    K = obj[k] #read kth frame
    Kplus = obj[k+1] #read k+1th frame
    #Using Sum histogram diff as Ranking, others are: Minkowski, helliger
    #distances...
    sss = absdif(K,Kplus) #calculate sum of different histogram between K, Kplus
    X.append(sss); #Save to array X
    retk=RET[k+2]
    k=k+1
    if time.time()-t>1:
        a=np.ceil(k*100*1.0/(num_f-2))
        b = ("calculating ... " + "%d"%a+"%")
        sys.stdout.write('\r'+b)
        t=time.time()
sys.stdout.write('\r'+"calculating ... 100%")
print ""
    
    

#RANKING BASE ON HIST SUM
I=np.argsort(X)
J = I[len(I) - num_key : len(I)]
J = sorted(J)
#cwd = os.getcwd()
t=time.time()
for i in range(len(J)):
    index = J[i]
    K = obj[index]
    #Write images
    #cv2.imwrite(cwd+'\\'+'frame'+str(index)+'.jpg',K)
    #Write video
    obj_sum.write(K)
    if time.time()-t>1:
        a=np.ceil(k*100*1.0/(num_f-2))
        b = ("writing video ... " + "%d"%a+"%")
        sys.stdout.write('\r'+b)
        t=time.time()
sys.stdout.write('\r'+"writing video ... 100%") 
print ""
time.sleep(1)
obj_sum.release()
print "Done"
time.sleep(1)


##RANKING BASE ON STD AND MEAN
#mean = np.mean(X)
#std = np.std(X)
#threshold = std + mean*2 #Set threshold base on mean and std
##Loop over all frame again
#cwd = os.getcwd()
#for k in range(num_f-1):
#    if X[k] > std:
#        K = obj[k]
#        #cv2.imwrite(cwd + '\\' + str(k) + '.jpg', K)
##This function actually detects video-shot.