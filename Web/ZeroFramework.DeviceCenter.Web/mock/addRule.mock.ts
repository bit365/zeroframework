// @ts-ignore
import { Request, Response } from 'express';

export default {
  'POST /api/rule': (req: Request, res: Response) => {
    res.status(200).send({
      key: 78,
      disabled: false,
      href: 'https://umijs.org/',
      avatar: 'https://avatars0.githubusercontent.com/u/507615?s=40&v=4',
      name: '蒋娜',
      owner: 'Williams',
      desc: '快严或长增导感分风即料到家时真后更。',
      callNo: 96,
      status: 69,
      updatedAt: 'Z6nd',
      createdAt: 'MYmu',
      progress: 68,
    });
  },
};
