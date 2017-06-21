# -*- coding: utf-8 -*-
"""
Created on Mon Jun 12 11:21:21 2017

@author: PhucMinh
"""

import cv2
import numpy as np
from matplotlib import pyplot as plt
import time
import sys

def smooth5(x,boo):
    y=[]
    n=len(x)
    if n<=2:
        return x
    if n>2:
        for i in range(n):
            if i==1 or i==n-2:
                y.append((x[i-1]+x[i]+x[i+1])/3)
            if i==0 or i==n-1:
                y.append(x[i])
            if i>=2 and i<=n-3:
                y.append((x[i-2]+x[i-1]+x[i]+x[i+1]+x[i+2])/5)
            t=time.time()
            if time.time()-t>1 and boo==True:
                a=np.ceil(i*100*1.0/n)
                b = ("smoothening ... " + "%d"%a+"%")
                sys.stdout.write('\r'+b)
                t=time.time()
        if boo==True:
            sys.stdout.write('\r'+"smoothening ... 100%")
            print ""
            time.sleep(1)
        return np.array(y)

def findpeaks_minpeakdist5_npeaks100(x,boo):
    n=len(x)
    I = np.array(sorted(range(n), key=lambda k: x[k]), np.int)
    i=n-1
    iout=[]
    iin=[]
    t=time.time()
    while i>=0:
        if i not in iout:
            for j in range(1,5):
                indplus=I[i]+j
                indminus=I[i]-j
                if indplus>=0 and indplus<=n-1:
                    iout.append(np.where(I==indplus)[0][0])
                if indminus>=0 and indminus<=n-1:
                    iout.append(np.where(I==indminus)[0][0])
            iin.append(I[i])
        i=i-1
        if boo==True and time.time()-t>1:
            a=100-np.ceil(i*100*1.0/(n-1))
            b = ("finding peaks ... " + "%d"%a+"%")
            sys.stdout.write('\r'+b)
            t=time.time()
    if boo==True:
        sys.stdout.write('\r'+"finding peaks ... 100%")
        print ""
        time.sleep(1)
        
        
    c=max(100,len(iin))
    iin=iin[0:c]
    y=[]
    iin=np.sort(iin)
    for i in iin:
        y.append(x[i])
    return np.array(y),iin
    
#Example: Estimate optical flow.
vid = cv2.VideoCapture('dance.mp4')
width = int(vid.get(cv2.CAP_PROP_FRAME_WIDTH))
height = int(vid.get(cv2.CAP_PROP_FRAME_HEIGHT))
num_f = int(vid.get(cv2.CAP_PROP_FRAME_COUNT)) #Number of frames
obj = []
t=time.time()
for i in range(num_f):
    ret, frame = vid.read()
    obj.append(frame)
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


frame=cv2.cvtColor(obj[0],cv2.COLOR_BGR2GRAY)
prvs=np.zeros_like(frame)
nxt=frame
vxy=[]
t=time.time()
for i in range(num_f-1):
    #Compute optical flow
    flow=cv2.calcOpticalFlowFarneback(prvs,nxt,None,0.5,3,15,3,5,1.2,0)
    prvs=cv2.cvtColor(obj[i],cv2.COLOR_BGR2GRAY)
    nxt=cv2.cvtColor(obj[i+1],cv2.COLOR_BGR2GRAY)
    #Calculate abs of Vx, Vy
    vxy.append(np.sum(np.abs(flow)))
    if time.time()-t>1:
        a=np.ceil(i*100*1.0/(num_f-1))
        b = ("calculating optical flow ... " + "%d"%a+"%")
        sys.stdout.write('\r'+b)
        t=time.time()
sys.stdout.write('\r'+"calculating optical flow ... 100%")
print ""   
flow=cv2.calcOpticalFlowFarneback(prvs,nxt,None,0.5,3,15,3,5,1.2,0)
vxy.append(np.sum(np.abs(flow)))
Vxy=np.array(vxy)
#Inverse to find peak instead of local mins
Y=np.max(Vxy)-Vxy
#Smooth data with 5 point moving average
Y=smooth5(Y, True)
#Find peaks' index
[peaks, index]=findpeaks_minpeakdist5_npeaks100(Y, True)
#Show peak graph
Y_index=[]
for i in index:
    Y_index.append(Y[i])
plt.figure(1)
plt.plot(range(num_f),Y)
plt.plot(index,Y_index,'go')
#Show local minimum point
Vxy_index=[]
for i in index:
    Vxy_index.append(Vxy[i])
plt.figure(2)
plt.plot(range(num_f),Vxy)
plt.plot(range(num_f),smooth5(Vxy, False))
plt.plot(index, Vxy_index, 'rx')
plt.xlabel('Frame number')
plt.ylabel('Sum of motion vecs')

t=time.time()
for i in range(len(index)):
    ind=index[i]
    frame=obj[ind]
    #Write video
    obj_sum.write(frame)
    if time.time()-t>1:
        a=np.ceil(i*100*1.0/len(index))
        b = ("writing video ... " + "%d"%a+"%")
        sys.stdout.write('\r'+b)
        t=time.time()
sys.stdout.write('\r'+"writing video ... 100%")
print ""
obj_sum.release()
print "Done"
time.sleep(1)


#y=np.array([551151,1,8,651,32,515,9550,9951,6,48])
#s=np.sort(y)
#print y
#print s
#print np.argsort(y)
#print np.where(y==651)[0][0]
#
#print findpeaks_minpeakdist5_npeaks100(y)
