// @ts-ignore
import { Request, Response } from 'express';

export default {
  'POST /api/rule': (req: Request, res: Response) => {
    res
      .status(200)
      .send({
        key: 86,
        disabled: false,
        href: 'https://github.com/umijs/dumi',
        avatar: 'https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png',
        name: '邹伟',
        owner: 'Thomas',
        desc: '采程着中感花又可土一例上属。',
        callNo: 72,
        status: 95,
        updatedAt: 'jmi',
        createdAt: 'Jg39xK&',
        progress: 83,
      });
  },
};
