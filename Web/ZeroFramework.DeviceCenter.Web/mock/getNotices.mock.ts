// @ts-ignore
import { Request, Response } from 'express';

export default {
  'GET /api/notices': (req: Request, res: Response) => {
    res.status(200).send({
      data: [
        {
          id: 'Ce1f8439-CA6a-783d-dfDC-1696e67ae8bD',
          extra: '^p6etT',
          key: 12,
          read: true,
          avatar: '',
          title: '外界二府建音须任五活响别期且等思部据。',
          status: 'error',
          datetime: '1984-04-27',
          description: '历快没战三代种天红段本类就更。',
          type: 'notification',
        },
      ],
      total: 72,
      success: true,
    });
  },
};
