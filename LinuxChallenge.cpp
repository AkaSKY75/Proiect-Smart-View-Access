#include <iostream>

using namespace std;

int v[10];

bool valid(int k)
{

}

int backtr()
{
    int k;
    bool cont;
    k = 0;
    v[k] = -1;
    while(k < 9)
    {
        cont = false;
        while(v[k] < 46 && !cont)
        {
            v[k]++;
            if(valid(k))
                cont = true;
        }
        if(cont == false)
            k--;
        else if(k == 8)
            return 1;
        else
        {
            k++;
            v[k] = -1;
        }
    }
}

int main()
{
    return 0;
}
