#include "cocos2d.h"
#include "utilities.h"

class Foreground : public cocos2d::Layer {
private:
  cocos2d::MenuItem* closeItem;
  cocos2d::Menu*     menu;
  cocos2d::Label*    label;

public:
  void update(float dt) {}
  virtual bool init();
  void menuCloseCallback(cocos2d::Ref* pSender);
  CREATE_FUNC(Foreground);
};
