#include "cocos2d.h"

extern cocos2d::Vec2 origin;
extern cocos2d::Size visibleSize;

cocos2d::Sprite* stretchFit(cocos2d::Sprite* const sprite);

float width (const cocos2d::Sprite* const sprite);
float height(const cocos2d::Sprite* const sprite);

float getRandFloat(float, float);
float getRandInRange(int, int);
