// @ts-ignore
import { Request, Response } from 'express';

export default {
  'GET /api/currentUser': (req: Request, res: Response) => {
    res.status(200).send({
      name: '秦艳',
      avatar: 'https://gw.alipayobjects.com/zos/rmsportal/udxAbMEhpwthVVcjLXik.png',
      userid: 'f5F1e34D-69bD-cd11-09Aa-C34C5AEc1836',
      email: 'b.sjdilxyyq@piqwft.bf',
      signature: '众持置写越来广克求第干数。',
      title: '派器两用斗战年满级林就流传。',
      group: '服务技术部',
      tags: [
        { key: 1, label: '程序员' },
        { key: 2, label: '小清新' },
        { key: 3, label: '大咖' },
        { key: 4, label: 'IT 互联网' },
        { key: 5, label: '阳光少年' },
        { key: 6, label: '海纳百川' },
        { key: 7, label: '健身达人' },
        { key: 8, label: '小清新' },
      ],
      notifyCount: 68,
      unreadCount: 65,
      country: '澳大利亚',
      access: '于领型院方经积县例装种据界。',
      geographic: { province: { label: '贵州省', key: 9 }, city: { label: '阜阳市', key: 10 } },
      address: '西藏自治区 拉萨市 堆龙德庆县',
      phone: '11471940593',
    });
  },
};
