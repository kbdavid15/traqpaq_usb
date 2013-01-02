'''
Created on Sep 20, 2012

@author: tew
'''
import random
f = open('../flinttrack.csv', 'r')

#read file into a 2d array, splitting on comma separation
lines = f.readlines()

cells = []

for line in lines:
    cells.append(line.split(','))

latitudes, longitudes, lat1, long1, lat2, long2 = [], [], [], [], [], []

for row in cells[21:130]:
    latitudes.append(float(row[2]))
    longitudes.append(float(row[3]))
    lat1.append(float(row[2]) + random.uniform(0, 0.001))
    long1.append(float(row[3]) + random.uniform(0, 0.001))
    lat2.append(float(row[2]) + random.uniform(0, 0.001))
    long2.append(float(row[3]) + random.uniform(0, 0.001))
print(lat1)
print(long1)
print(lat2)
print(long2)
input()
