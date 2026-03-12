#include<iostream>
#include<cmath>
using namespace std;
bool shu(int n) {
	int sqrtn = (int)sqrt(n);
	for (int i = 2; i <= sqrtn; i++) {
		if (n % i == 0)return true;
	}
	return false;
}
int main() {
	int high, low;
	cin >> high >> low;
	int num = 0;
	for (int i = low; i <= high; i++) {
		if (!shu(i)) {
			cout << i << ' ';
			num++;
		}
		if (num == 10) {
			cout << endl;
			num = 0;
		}
	}
	return 0;
}