'''
Created on Jan 1, 2013

@author: Kyle David
'''

f = open('../cardrivedata.csv', 'r')

#read file into a 2d array, splitting on comma separation
lines = f.readlines()

f.close()   #close file

cells = []

for line in lines:
    cells.append(line.split(','))

latitudes, longitudes, altitudes, speeds = [], [], [], []

for row in cells:
    latitudes.append(float(row[2]))
    longitudes.append(float(row[1]))
    altitudes.append(float(row[3]))
    speeds.append(float(row[4]))
    
print(latitudes)
print(longitudes)
print(altitudes)
print(speeds)
input()
