// @ts-ignore
import { Request, Response } from 'express';

export default {
  'GET /api/currentUser': (req: Request, res: Response) => {
    res.status(200).send({
      name: '杨超',
      avatar: '',
      userid: '797eC7D9-E6C5-51c4-B7e1-c3925B55abAC',
      email: 's.wbgs@abll.aw',
      signature: '论叫个才然力布机习制因识争议新世六。',
      title: '电团期族今办想三将构众族般本对没。',
      group: '区块链平台部',
      tags: [
        { key: 1, label: '很有想法的' },
        { key: 2, label: '小清新' },
        { key: 3, label: '程序员' },
        { key: 4, label: '名望程序员' },
        { key: 5, label: '小清新' },
        { key: 6, label: '专注设计' },
        { key: 7, label: '健身达人' },
        { key: 8, label: '健身达人' },
        { key: 9, label: '傻白甜' },
      ],
      notifyCount: 81,
      unreadCount: 81,
      country: '土耳其',
      access: '连别争你物数体发于命上厂动定。',
      geographic: {
        province: { label: '浙江省', key: 10 },
        city: { label: '玉树藏族自治州', key: 11 },
      },
      address: '吉林省 松原市 扶余市',
      phone: '11465575063',
    });
  },
};
