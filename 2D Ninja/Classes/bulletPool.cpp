#include "bulletPool.h"

void BulletPool::createBullet(const cocos2d::Vec2& playerPos)
{
  if (freeList.empty())
  {
    BasicSprite* newbullet = new BasicSprite(cocos2d::Vec2(300, 0), cocos2d::Vec2(playerPos.x,playerPos.y), "lantern.png");
    bulletList.push_back(newbullet);
    (newbullet->getSprite())->retain();
  }
  else
  {
    BasicSprite* bullet = freeList.front();
    freeList.pop_front();
    bulletList.push_back(bullet);
    bullet->reset(cocos2d::Vec2(playerPos.x, playerPos.y));
  }
}

void BulletPool::update(float dt)
{
  std::list<BasicSprite*>::iterator ptr = bulletList.begin();
  while (ptr != bulletList.end())
  {
    if (!(*ptr)->getIsRendered())
    {
      parent->addChild((*ptr)->getSprite());
      (*ptr)->getSprite()->release();
      (*ptr)->setIsRendered(true);
    }
    (*ptr)->update(dt);

    /*std::list<Drawable*>::iterator ptrEnemy = sprites.begin();
    while (ptrEnemy != sprites.end())
    {
      //-
      if ((*ptr)->collidedRect(*ptrEnemy)) {
        if ((*ptrEnemy)->getIsExplodable() && !(*ptrEnemy)->getIsRemovable())
        {
          (*ptrEnemy)->makeChunks(sprites);
          (*ptrEnemy)->setIsRemovable(true);
        }
        break;
      }

      ++ptrEnemy;
      //--
    }*/

    if ((*ptr)->getIsRemovable())
    {
      (*ptr)->getSprite()->retain();
      parent->removeChild((*ptr)->getSprite());
      freeList.push_back((*ptr));
      ptr = bulletList.erase(ptr);
    }
    else
      ++ptr;
  }
}
