import numpy as np
import matplotlib.pyplot as plt
data = np.loadtxt("trace.txt", delimiter=",")
print(data.size)
plt.imshow(data, cmap='hot', interpolation='nearest')
plt.show()
