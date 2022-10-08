// @ts-ignore
import { Request, Response } from 'express';

export default {
  'PUT /api/rule': (req: Request, res: Response) => {
    res
      .status(200)
      .send({
        key: 95,
        disabled: false,
        href: 'https://procomponents.ant.design/',
        avatar: 'https://gw.alipayobjects.com/zos/rmsportal/OKJXDXrmkNshAMvwtvhu.png',
        name: '傅超',
        owner: 'Gonzalez',
        desc: '号来联很治际决并系毛圆研保也情属。',
        callNo: 72,
        status: 76,
        updatedAt: '11y6Et',
        createdAt: 'dw7Skz',
        progress: 81,
      });
  },
};
