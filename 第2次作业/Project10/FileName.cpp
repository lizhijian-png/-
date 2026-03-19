#include <iostream>
#include <cmath>
#include <string>
#include <iomanip>

using namespace std;

//抽象基类：形状
class Shape {
public:
    
    virtual ~Shape() {}

    
    virtual double calculateArea() const = 0;

    //形状合法性判断
    virtual bool isValid() const = 0;

    // 获取形状名称（方便打印）
    virtual std::string getName() const = 0;
};

// 长方形类
class Rectangle : public Shape {
protected:
    double width;
    double height;

public:
    Rectangle(double w, double h) : width(w), height(h) {}

    bool isValid() const override {
        return width > 0 && height > 0;
    }

    double calculateArea() const override {
        return isValid() ? (width * height) : 0.0;
    }

    string getName() const override { return "Rectangle"; }
};

// 正方形类 (继承自长方形)
class Square : public Rectangle {
public:
    Square(double side) : Rectangle(side, side) {}

    string getName() const override { return "Square"; }
};

// 4. 圆形类
class Circle : public Shape {
private:
    double radius;
    const double PI = 3.1415926;

public:
    Circle(double r) : radius(r) {}

    bool isValid() const override {
        return radius > 0;
    }

    double calculateArea() const override {
        return isValid() ? (PI * radius * radius) : 0.0;
    }

    string getName() const override { return "Circle"; }
};


int main() {
    //创建形状
    Circle* c1 = new Circle(2.1);
    Circle* c2 = new Circle(4.0);
    Circle* c3 = new Circle(8.9);
    Square* s1 = new Square(5);
    Square* s2 = new Square(8.3);
    Square* s3 = new Square(4.9);
    Rectangle* r1 = new Rectangle(5, 4);
    Rectangle* r2 = new Rectangle(6, 3);
    Rectangle* r3 = new Rectangle(5.4, 3.7);
    Rectangle* r4 = new Rectangle(6.9, 3);
    //计算面积和
    double area = 0;
    area += c1->calculateArea();
    area += c2->calculateArea();
    area += c3->calculateArea();
    area += s1->calculateArea();
    area += s2->calculateArea();
    area += s3->calculateArea();
    area += r4->calculateArea();
    area += r1->calculateArea();
    area += r2->calculateArea();
    area += r3->calculateArea();
    //输出结果，保留4位小数
    cout << "10个图形面积总和为：" << fixed << setprecision(4) << area;
    delete c1;
    delete c2;
    delete c3;
    delete s1;
    delete s2;
    delete s3;
    delete r1;
    delete r2;
    delete r3;
    delete r4;

    return 0;
}