#include "basicSprite.h"
#include <list>
class BulletPool
{
public:
  BulletPool() {}
  void createBullet(const cocos2d::Vec2& playerPos);
  void setParentLayer(cocos2d::Layer* &p) { parent = p; }
  void update(float dt);
private:
  std::list<BasicSprite*> bulletList;
  std::list<BasicSprite*> freeList;
  cocos2d::Layer* parent;
  BulletPool(const BulletPool&);
  BulletPool& operator=(const BulletPool&);
};
